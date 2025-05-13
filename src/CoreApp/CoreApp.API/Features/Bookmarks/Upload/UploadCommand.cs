using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Features.Bookmarks.Dtos;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using FluentValidation;
using HtmlAgilityPack;
using MediatR;

namespace CoreApp.API.Features.Bookmarks.Upload;

public record UploadCommand(UploadRequest File) : IRequest;


public class UploadCommandHandler
{
 
  public class UploadValidator : AbstractValidator<UploadRequest>
  {
    public UploadValidator()
    {
      RuleFor(x => x.FileName).NotNull().NotEmpty();
      // TODO: check type of file
      RuleFor(x => x.FileContent).NotNull().NotEmpty();
      RuleFor(x => x.UploadTimestamp).NotNull().NotEmpty();
    }
  }

  public class CommandValidator : AbstractValidator<UploadCommand>
  {
    public CommandValidator() =>
        RuleFor(x => x.File).NotNull().SetValidator(new UploadValidator());
  }

  public class Handler : IRequestHandler<UploadCommand>
  {
    private readonly CoreAppContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;


    public Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor)
    {
      _context = context;
      _currentUserAccessor = currentUserAccessor;
    }


    public async Task Handle(
        UploadCommand command,
        CancellationToken cancellationToken
    )
    {
      // Read the file content into a byte array (if needed)
      using var memoryStream = new MemoryStream();
      await command.File.FileContent.CopyToAsync(memoryStream);
      var fileBytes = memoryStream.ToArray();

      string htmlContentString = System.Text.Encoding.UTF8.GetString(fileBytes);

      // Remove all <p> nodes
      string cleanedHtml = CleanHtml(htmlContentString);

      // TODO: Map to model of html content
      BookmarkFolderDto rootFolder = ParseBookmarks(cleanedHtml);

      // Iterate all folders
      // TODO: the method its not mapping correctly, use an iterate method and then AI to generate suggested folders?
      await SaveFoldersAndBookmarksToDatabase(rootFolder, 0, _context, cancellationToken);

      /* TODO: Processes content

      1- Remove duplicates
      2- Group by similars (create folders and add same page / content to that folder)
      3- Order
      4- Add to database (store folders and its corresponding bookmarks
      5- Add to database (store file content table / metrics)

      */

    }
  }

  public static string CleanHtml(string htmlContent)
  {

    return htmlContent.Replace("<p>", string.Empty)
                      .Replace("<P>", string.Empty)
                      .Replace("</P>", string.Empty)
                      .Replace("</p>", string.Empty);

  }


  private static BookmarkFolderDto ParseBookmarks(string html)
  {
    var document = new HtmlDocument();
    document.LoadHtml(html);

    var rootFolder = new BookmarkFolderDto
    {
      Title = "Root"
    };
    var dlNode = document.DocumentNode.SelectSingleNode("//dl");
    if (dlNode != null)
    {
      ParseFolder(dlNode, rootFolder);
    }

    return rootFolder;
  }

  private static void ParseFolder(HtmlNode dlNode, BookmarkFolderDto parentFolder)
  {
    foreach (var node in dlNode.ChildNodes)
    {
      if (node.Name == "dt")
      {
        var h3Node = node.SelectSingleNode("h3");
        if (h3Node != null)
        {
          // Parse folder
          string addDateValue = h3Node.GetAttributeValue("add_date", string.Empty);
          string lastModifiedDateValue = h3Node.GetAttributeValue("last_modified", string.Empty);

          var folder = new BookmarkFolderDto
          {
            Title = h3Node.InnerText,
            AddDate = string.IsNullOrWhiteSpace(addDateValue) ? null : ParseUnixTimestamp(addDateValue),
            LastModified = string.IsNullOrWhiteSpace(lastModifiedDateValue) ? null : ParseUnixTimestamp(lastModifiedDateValue),
          };

          var subDlNode = node.ChildNodes
           .FirstOrDefault(n => string.Equals(n.Name, "dl", StringComparison.OrdinalIgnoreCase));

          if (subDlNode != null)
          {
            ParseFolder(subDlNode, folder);
          }

          parentFolder.SubFolders.Add(folder);
        }

        var aNode = node.SelectSingleNode("a");
        if (aNode != null)
        {
          // Parse bookmark
          string addDateValue = aNode.GetAttributeValue("add_date", string.Empty);

          var bookmark = new BookmarkDto
          {
            Title = aNode.InnerText,
            Url = aNode.GetAttributeValue("href", string.Empty),
            AddDate = string.IsNullOrWhiteSpace(addDateValue) ? null : ParseUnixTimestamp(addDateValue),
            Icon = aNode.GetAttributeValue("icon", string.Empty)
          };

          parentFolder.Bookmarks.Add(bookmark);
        }
      }
    }
  }

  private static DateTime? ParseUnixTimestamp(string timestamp)
  {
    if (long.TryParse(timestamp, out var unixTime))
    {
      return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
    }

    return null;
  }

  private static async Task SaveFoldersAndBookmarksToDatabase(
    BookmarkFolderDto folder,
    int fatherId,
    CoreAppContext context,
    CancellationToken cancellationToken)
  {
    // Save the folder
    var dbFolder = new BookmarkFolder
    {
      Title = folder.Title,
      AddDate = folder.AddDate,
      LastModified = folder.LastModified,
      IsPersonalToolbarFolder = false
    };

    await context.BookmarkFolders.AddAsync(dbFolder, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    // Save the bookmarks in the folder
    foreach (var bookmark in folder.Bookmarks)
    {
      var dbBookmark = new Bookmark
      {
        Title = bookmark.Title,
        Url = bookmark.Url,
        AddDate = bookmark.AddDate,
        Icon = bookmark.Icon,
        BookmarkFolderId = dbFolder.Id,
      };

      await context.Bookmarks.AddAsync(dbBookmark, cancellationToken);
    }

    await context.SaveChangesAsync(cancellationToken);

    // Recursively save subfolders
    foreach (var subFolder in folder.SubFolders)
    {
      await SaveFoldersAndBookmarksToDatabase(subFolder, dbFolder.Id, context, cancellationToken);
    }
  }


}
