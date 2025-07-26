using Amazon.Runtime.Internal;
using CoreApp.API.Features.Bookmarks.Dtos;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.ollama;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using CoreApp.API.MessageBrokers.Messages;
using HtmlAgilityPack;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Wolverine;
using static CoreApp.API.MessageBrokers.Producers.BookmarksMessageProducer;

namespace CoreApp.API.MessageBrokers.Consumers;

public class BookmarksConsumer
{
  private readonly CoreAppContext _context;
  private readonly IOllamaService _ollamaService;

  public BookmarksConsumer(CoreAppContext context, IOllamaService ollamaService)
  {
    _context = context;
    _ollamaService = ollamaService;
  }

  public async Task Consume(UploadBookmarksMessageRequest message, CoreAppContext context, IOllamaService ollamaService)
  {
    try
    {
      List<BookmarkDto> uploadedBookmarks = ParseAllBookmarks(message.HtmlContent);

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

      // Update a database record about the upload status.
      // or
      // Notify the original client via WebSockets(e.g., using SignalR).

    }
    catch (Exception ex)
    {
      // Log the error
      Console.WriteLine($"Error processing upload {message.UploadId}: {ex.Message}");
      result = new UploadProcessingResult
      {
        UploadId = message.UploadId,
        IsSuccess = false,
        ErrorMessage = ex.Message
      };
    }
    finally
    {
      // Hnadle retry if fail?
    }

  }


  // New method to consume DeleteBookmarksMessageRequest
  public async Task Handle(DeleteBookmarksMessageRequest message) // Can use Handle, Consume, etc.
  {
    _logger.LogInformation("Processing delete bookmarks request for User: {UserId}, Bookmarks: {BookmarkIds}",
                           message.UserId, string.Join(", ", message.BookmarkIds));
    try
    {
      // Logic to delete bookmarks from the database
      await Task.Delay(30); // Simulate deletion work

      _logger.LogInformation("Bookmarks deleted for User: {UserId}", message.UserId);
      // You could cascade an event here too, e.g., BookmarksDeletedEvent
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error deleting bookmarks for User: {UserId}", message.UserId);
    }
  }

  #region Helpers 


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

  private static List<BookmarkDto> RemoveDuplicateBookmarksByUrl(List<BookmarkDto> bookmarks)
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

  #endregion
}
