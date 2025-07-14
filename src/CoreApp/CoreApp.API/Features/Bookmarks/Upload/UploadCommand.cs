using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Features.Bookmarks.Dtos;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.ollama;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using FluentValidation;
using HtmlAgilityPack;
using Mediator;

namespace CoreApp.API.Features.Bookmarks.Upload;

public sealed record UploadCommand(UploadRequest File) : IQuery<UploadResponse>;


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

  public sealed class Handler : IQueryHandler<UploadCommand, UploadResponse> 
  {
    private readonly CoreAppContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    private readonly IOllamaService _ollamaService;


    public Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor, IOllamaService ollamaService)
    {
      _context = context;
      _currentUserAccessor = currentUserAccessor;
      _ollamaService = ollamaService;
    }


    public async ValueTask<UploadResponse> Handle(
        UploadCommand command,
        CancellationToken cancellationToken
    )
    {
      // TODO: Move all logic to a separate service and the endpoint will send a event and return in progress, the service
      // will process and update the event with fail with reason or success with result

      // Use mass transit alternative, Wolverine https://wolverinefx.net/guide/messaging/message-bus.html
      // https://medium.com/@ms111mithun/mastering-message-queues-leveraging-rabbitmq-locally-and-azure-service-bus-in-production-for-net-277236f25609

      // Where send the html file generated with the grouped bookmarks?


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

      // Add Id to each bookmark
      for (int i = 0; i < uploadedBookmarks.Count; i++)
      {
        uploadedBookmarks[i].Id = i + 1;
      }

      // Store bookmarks icon property in a dictionary with the key set using the Id
      var iconsDictionary = uploadedBookmarks.ToDictionary(b => b.Id, b => b.Icon);

      // Read json file to obtain the desired output structure
      string pathToJson = Path.Combine(AppContext.BaseDirectory, "Config", "ollama", "format_output.json");
      string jsonFormatOutput = File.ReadAllText(pathToJson);
      JsonElement formatElement = JsonSerializer.Deserialize<JsonElement>(jsonFormatOutput);

      var options = new System.Text.Json.JsonSerializerOptions
      {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
      };

      var simplified = uploadedBookmarks.Select(b => new { b.Id, b.Title, b.Url }).ToList();
      var jsonData = JsonSerializer.Serialize(simplified, options);

      var prompt = @$"I have a list of bookmarks in JSON format. Each bookmark has the following properties:

Id: a unique identifier
Title: the title of the bookmark
Url: the full URL
Please organize these bookmarks into folders based on domain or content similarity.
If there is a large group of closely related bookmarks within a folder,
you may optionally create subfolders for more granular organization.

Constraints:
Uniqueness: Each bookmark must appear only once in the final result. Use the Id to ensure no duplicates across folders or collections.
Folder Assignment:
If a bookmark has related bookmarks (by domain or topic), group them together in a folder.
If a bookmark has no related items, place it in a separate collection called withoutFolder.
Subfolders:
Only create subfolders if there are enough related bookmarks (e.g., 3 or more) that justify a more detailed grouping.
Output Format: Return the result as a JSON object.
Here is the data: {jsonData}";

      var requestAI = new ProcessBookmarkGroupingRequest()
      {
        Model = "llama3.2:3b",
        Prompt = prompt,
        Stream = false,
        Format = formatElement
      };

      var responseOllama = await _ollamaService.ProcessBookmarksGroupingAsync(requestAI, cancellationToken);

      foreach (var bookmark in responseOllama.WithoutFolder)
      {
        if (iconsDictionary.TryGetValue(bookmark.Id, out var icon))
        {
          bookmark.Icon = icon;
        }
      }

      foreach (var folder in responseOllama.WithFolder)
      {
        SetIconsInFolder(folder, iconsDictionary);
      }

      // Save request file in storage like Blob or Minio

      // var fileBytes = memoryStream.ToArray();



      // Convert response to html file with import format


      return new UploadResponse();
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

    private static void SetIconsInFolder(BookmarkFolderDto folder, Dictionary<int, string?> iconsDictionary)
    {
      foreach (var bookmark in folder.Bookmarks)
      {
        if (iconsDictionary.TryGetValue(bookmark.Id, out var icon))
        {
          bookmark.Icon = icon;
        }
      }

      foreach (var subFolder in folder.SubFolders)
      {
        SetIconsInFolder(subFolder, iconsDictionary);
      }
    }

  }
}
