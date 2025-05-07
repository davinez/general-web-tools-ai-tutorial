using System;
using System.Collections.Generic;
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

public class Upload
{
  public record Command(UploadRequest File) : IRequest<UploadResponse>;

  public class UploadValidator : AbstractValidator<UploadRequest>
  {
    public UploadValidator()
    {
      RuleFor(x => x.FileName).NotNull().NotEmpty();
      RuleFor(x => x.FileContent).NotNull().NotEmpty();
      RuleFor(x => x.UploadTimestamp).NotNull().NotEmpty();
    }
  }

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator() =>
        RuleFor(x => x.File).NotNull().SetValidator(new UploadValidator());
  }

  public class Handler : IRequestHandler<Command, UploadResponse>
  {
    private readonly CoreAppContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;


    public Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor)
    {
      _context = context;
      _currentUserAccessor = currentUserAccessor;
    }


    public async Task Handle(
        Command command,
        CancellationToken cancellationToken
    )
    {
      // TODO: Map byte file to string and check type of file
      string htmlContentString = System.Text.Encoding.UTF8.GetString(command.File.FileContent);


      // TODO: Map to model of html content
      BookmarkFolderDto rootFolder = ParseBookmarks(htmlContentString);

      // Iterate all folders
      foreach (var folder in (rootFolder. ?? Enumerable.Empty<string>()))
      {
        var t = await context.Tags.FindAsync(tag);
        if (t == null)
        {
          t = new Tag { TagId = tag };
          await context.Tags.AddAsync(t, cancellationToken);
          //save immediately for reuse
          await context.SaveChangesAsync(cancellationToken);
        }
        tags.Add(t);
      }


      var bookmark = new Article
      {
        Author = author,
        Body = message.Article.Body,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Description = message.Article.Description,
        Title = message.Article.Title,
        Slug = message.Article.Title.GenerateSlug()
      };
      await context.Articles.AddAsync(article, cancellationToken);

      await context.ArticleTags.AddRangeAsync(
          tags.Select(x => new ArticleTag { Article = article, Tag = x }),
          cancellationToken
      );

      await context.SaveChangesAsync(cancellationToken);

      /* TODO: Processes content

      1- Remove duplicates
      2- Group by similars (create folders and add same page / content to that folder)
      3- Order
      4- Add to database (store folders and its corresponding bookmarks
      5- Add to database (store file content table / metrics)

      */

    }
  }


  private static BookmarkFolderDto ParseBookmarks(string html)
  {
    var document = new HtmlDocument();
    document.LoadHtml(html);

    var rootFolder = new BookmarkFolderDto
    {
      Title = "Root",
      SubFolders = new List<BookmarkFolderDto>()
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
            IsPersonalToolbarFolder = h3Node.GetAttributeValue("personal_toolbar_folder", "false") == "true"
          };

          var subDlNode = node.SelectSingleNode("following-sibling::dl");
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
}
