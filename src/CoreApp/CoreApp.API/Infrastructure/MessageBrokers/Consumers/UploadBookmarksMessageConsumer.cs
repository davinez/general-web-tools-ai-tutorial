using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.Hubs;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Endpoints.Bookmarks.Dtos;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;
using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using CoreApp.API.Infrastructure.Hubs;
using CoreApp.API.Infrastructure.Hubs.Dto;
using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using CoreApp.API.Utils;
using HtmlAgilityPack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static CoreApp.API.Domain.Constants.StatusConstants;

namespace CoreApp.API.Infrastructure.MessageBrokers.Consumers;

public class UploadBookmarksMessageConsumer
{
  private readonly ILogger<UploadBookmarksMessageConsumer> _logger;
  private readonly CoreAppContext _context;
  private readonly IAiService _aiService;
  private readonly IStorageService _storageService;
  private readonly IHubContext<JobEventStatusHub, IJobEventStatusHub> _hubContext;

  public UploadBookmarksMessageConsumer(
    ILogger<UploadBookmarksMessageConsumer> logger,
    CoreAppContext context,
    IAiService aiService,
    IStorageService storageService,
    IHubContext<JobEventStatusHub, IJobEventStatusHub> hubContext)
  {
    _logger = logger;
    _context = context;
    _aiService = aiService;
    _storageService = storageService;
    _hubContext = hubContext;
  }

  public async Task Consume(UploadBookmarksMessageRequest message, CancellationToken cancellationToken)
  {
    string messageStatus = string.Empty;

    try
    {
      // SETUP & PRE-PROCESSING
      // =================================================================

      List<BookmarkDto> uploadedBookmarks = ParseAllBookmarks(message.HtmlContent);
      uploadedBookmarks = RemoveDuplicateBookmarksByUrl(uploadedBookmarks);
      // Add Id to each bookmark
      for (int i = 0; i < uploadedBookmarks.Count; i++)
      {
        uploadedBookmarks[i].Id = i + 1;
      }
      // Store bookmarks icon property in a dictionary with the key set using the Id
      var iconsDictionary = uploadedBookmarks.ToDictionary(b => b.Id, b => b.Icon);

      // We only need a simplified version for the AI to process.
      var simplifiedBookmarks = uploadedBookmarks
          .Select(b => new { b.Id, b.Title, b.Url })
          .ToList();
      var allCategorizedBookmarks = new List<CategorizationResponse>();
      const int batchSize = 100;

      // PHASE 1: CATEGORIZE IN BATCHES
      // =================================================================

      // Use .Chunk() to easily create batches (.NET 6+).
      foreach (var batch in simplifiedBookmarks.Chunk(batchSize))
      {
        cancellationToken.ThrowIfCancellationRequested();

        // The JSON payload for this specific batch.
        var jsonDataForBatch = JsonSerializer.Serialize(batch);

        string prompt = @$"For each bookmark in the JSON list below, suggest a single, concise folder name based on its title and URL. 
Group related topics or series under the exact same folder name (e.g., use 'One Piece' for all chapters of that manga). 
If a bookmark is unique, suggest 'Miscellaneous'.
Return ONLY a JSON array of objects, where each object has the original 'Id' (int) and a new 'FolderName' (string) property.

Data:
{jsonDataForBatch}";

        try
        {
          List<CategorizationResponse> batchResult = await _aiService.CategorizeIntoFolderNameAsync(prompt, cancellationToken);
          if (batchResult != null)
          {
            allCategorizedBookmarks.AddRange(batchResult);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, $"{nameof(UploadBookmarksMessageConsumer)}:{nameof(Consume)}, Error processing a batch.");
          throw new CoreAppException($"An error occurred in {nameof(UploadBookmarksMessageConsumer)}:{nameof(Consume)} while processing a batch.", ex);
        }
      }

      // PHASE 2: CONSOLIDATE RESULTS
      // =================================================================

      // Create a lookup dictionary for easy access to the folder names.
      Dictionary<int, string> folderNameLookup = allCategorizedBookmarks.ToDictionary(r => r.Id, r => r.FolderName);

      var finalResponse = new BookmarkGroupingResponse
      {
        WithFolder = []
      };

      // Group the *original* bookmarks using the AI-generated folder names.
      IEnumerable<IGrouping<string, BookmarkDto>> groupedBookmarks = uploadedBookmarks
          .Where(b => folderNameLookup.ContainsKey(b.Id))
          .GroupBy(b => folderNameLookup[b.Id]);

      foreach (IGrouping<string, BookmarkDto> group in groupedBookmarks)
      {
        // Handle sub-folders by splitting on '/'
        string[] folderPath = group.Key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        // This logic adds bookmarks to a nested folder structure.
        AddBookmarksToNestedFolder(finalResponse.WithFolder, folderPath, group.ToList());
      }

      // =================================================================
      // Re-attach icons to the final, structured data.

      foreach (BookmarkFolderDto folder in finalResponse.WithFolder)
      {
        SetIconsInFolder(folder, iconsDictionary);
      }

      // =================================================================
      // Generate the HTML file from the final structured data.
      var generatorHtmlFile = new BookmarkHtmlGenerator();
      string bookmarkHtml = generatorHtmlFile.Generate(finalResponse.WithFolder, []);

      var memoryStream = new MemoryStream();
      await using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
      {
        await writer.WriteAsync(bookmarkHtml);
        await writer.FlushAsync(cancellationToken); // Ensure all content is written to the stream
      }
      memoryStream.Position = 0;

      var fileData = new FileDto()
      {
        FileName = $"{message.UploadId}.html",
        Content = memoryStream,
        ContentType = "text/html"
      };

      await _storageService.UploadFileAsync(fileData);

      var jobEvent = _context.JobEvents
                      .FirstOrDefault(je => je.JobEventId == message.UploadId);

      if (jobEvent == null)
      {
        messageStatus = JobStatus.Failed.ToString();
        throw new CoreAppException($"JobEvent with ID {message.UploadId} not found, in workflow ${Workflow.BookmarksUpload.ToString()}");
      }

      messageStatus = JobStatus.Complete.ToString();
      jobEvent.Status = messageStatus;
      await _context.SaveChangesAsync(cancellationToken);

    }
    catch (Exception ex)
    {
      messageStatus = JobStatus.Failed.ToString();

      _logger.LogError(ex, $"{nameof(UploadBookmarksMessageConsumer)}:{nameof(Consume)}, Critical error processing upload {message.UploadId}: {ex.Message}");

      // Handle final failure, update DB status to "Failed".
      var jobEvent = _context.JobEvents
                     .FirstOrDefault(je => je.JobEventId == message.UploadId);
      if (jobEvent != null)
      {
        jobEvent.Status = JobStatus.Failed.ToString();
        jobEvent.Content = JsonSerializer.Serialize(new { ErrorMessage = ex.Message }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await _context.SaveChangesAsync(cancellationToken);
      }
    }

    // Send a notification to the client 
    var hubUpdate = new JobEventStatusHubDto
    {
      JobId = message.UploadId.ToString(),
      Status = messageStatus,
      Timestamp = DateTime.UtcNow
    };
    await _hubContext.Clients.User(message.UserId).StatusUpdate(hubUpdate);
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

  // Helper method to handle nested folder creation.
  private static void AddBookmarksToNestedFolder(List<BookmarkFolderDto> folders, string[] path, List<BookmarkDto> bookmarks)
  {
    List<BookmarkFolderDto> currentFolders = folders;
    BookmarkFolderDto? targetFolder = null;

    foreach (string folderName in path)
    {
      targetFolder = currentFolders.FirstOrDefault(f => f.Title.Equals(folderName, StringComparison.OrdinalIgnoreCase));

      if (targetFolder == null)
      {
        targetFolder = new BookmarkFolderDto { Title = folderName, Bookmarks = new List<BookmarkDto>(), SubFolders = new List<BookmarkFolderDto>() };
        currentFolders.Add(targetFolder);
      }
      currentFolders = targetFolder.SubFolders;
    }

    if (targetFolder != null)
    {
      targetFolder.Bookmarks.AddRange(bookmarks);
    }
  }

  #endregion
}
