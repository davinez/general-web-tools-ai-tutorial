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
using CoreApp.API.Infrastructure.ExternalServices.Dto;
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

      List<BookmarkDto> uploadedBookmarks = ParseAllBookmarks(cleanedHtml);

      // Remove duplicates
      uploadedBookmarks = RemoveDuplicateBookmarksByUrl(uploadedBookmarks);

      var requestAI = uploadedBookmarks.Select(x =>
         {

           var routes = SearchForCurrentFolderRoute(x.Url);

           return new ProcessBookmarkGroupingDto
           {
             Title = x.Title,
             Url = x.Url,
             CurrentStructure = new CurrentStructure
             {
               RouteIds = routes.HasValue ? routes.Value.idRoute : "",
               RouteNames = routes.HasValue ? routes.Value.idRoute : ""
             }
           };
         })
          .ToList();


      // TODO: Call Ollama API


      // Iterate all folders
      /* TODO: Processes content

      1- Remove duplicates
      2- Group by similars (create folders and add same page / content to that folder)
      3- Order
      4- Add to database (store folders and its corresponding bookmarks
      5- Add to database (store file content table / metrics)
      */


      var options = new System.Text.Json.JsonSerializerOptions
      {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true // optional, for pretty print
      };
      string json = System.Text.Json.JsonSerializer.Serialize(uploadedBookmarks, options);

      // Save in DB
      // await SaveFoldersAndBookmarksToDatabase(rootFolder, 0, _context, cancellationToken);


    }


    public static string CleanHtml(string htmlContent)
    {

      return htmlContent.Replace("<p>", string.Empty)
                        .Replace("<P>", string.Empty)
                        .Replace("</P>", string.Empty)
                        .Replace("</p>", string.Empty);

    }


    public static List<BookmarkDto> ParseAllBookmarks(string html)
    {
      var document = new HtmlDocument();
      document.LoadHtml(html);
      var bookmarks = new List<BookmarkDto>();
      var aNodes = document.DocumentNode.SelectNodes("//a");
      if (aNodes != null)
      {
        foreach (var aNode in aNodes)
        {
          string addDateValue = aNode.GetAttributeValue("add_date", string.Empty);
          var bookmark = new BookmarkDto
          {
            Title = aNode.InnerText,
            Url = aNode.GetAttributeValue("href", string.Empty),
            AddDate = string.IsNullOrWhiteSpace(addDateValue) ? null : ParseUnixTimestamp(addDateValue),
            Icon = aNode.GetAttributeValue("icon", string.Empty)
          };
          bookmarks.Add(bookmark);
        }
      }
      return bookmarks;
    }

    public static List<BookmarkDto> RemoveDuplicateBookmarksByUrl(List<BookmarkDto> bookmarks)
    {
      return bookmarks
        .GroupBy(b => b.Url?.Trim().ToLowerInvariant())
        .Select(g => g.First())
        .ToList();
    }

    private static DateTime? ParseUnixTimestamp(string timestamp)
    {
      if (long.TryParse(timestamp, out var unixTime))
      {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
      }

      return null;
    }

    private (string idRoute, string nameRoute)? SearchForCurrentFolderRoute(string url)
    {
      // Find the bookmark in the DB by URL (case-insensitive, trimmed)
      var bookmark = _context.Bookmarks
        .FirstOrDefault(b => b.Url.Trim().Equals(url.Trim().ToLower(), StringComparison.CurrentCultureIgnoreCase));

      if (bookmark == null)
        return null;

      // Traverse up the folder tree to build the route
      var folderIds = new List<int>();
      var folderNames = new List<string>();

      BookmarkFolder? folder = _context.BookmarkFolders.First(f => f.Id == bookmark.BookmarkFolderId);

      while (folder != null)
      {

        // Insert(0, ...), which ensures the path is built in the correct order from root to leaf, rather than leaf to root.}
        // Inserts the new value at the very beginning (index 0) of the list, pushing all existing elements one position to the right.
        // If you used Add() instead, the order would be reversed.
        folderIds.Insert(0, folder.Id);
        folderNames.Insert(0, folder.Title);
        if (folder.ParentFolderId.HasValue)
          folder = _context.BookmarkFolders.FirstOrDefault(f => f.Id == folder.ParentFolderId.Value);
        else
          folder = null;
      }
      string idRoute = string.Join("/", folderIds);
      string nameRoute = string.Join("/", folderNames);
      return (idRoute, nameRoute);
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
        ParentFolderId = fatherId,
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
}
