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
using CoreApp.API.Infrastructure.ExternalServices.ollama;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using FluentValidation;
using HtmlAgilityPack;
using MediatR;
using NJsonSchema;

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

    private readonly IOllamaService _ollamaService;


    public Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor, IOllamaService ollamaService)
    {
      _context = context;
      _currentUserAccessor = currentUserAccessor;
      _ollamaService = ollamaService;
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


      // Read json file to obtain the desired output structure
      string pathToJson = Path.Combine(AppContext.BaseDirectory, "Config", "ollama", "format_output.json");
      string json = File.ReadAllText(pathToJson);

      //var requestAISchemaResponse = JsonSchema.FromType<ProcessBookmarkGroupingRequest>();
      //Console.WriteLine(requestAISchemaResponse.ToJson());

      // Test
      var options = new System.Text.Json.JsonSerializerOptions
      {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true // optional, for pretty print
      };
      string jsonData = System.Text.Json.JsonSerializer.Serialize(uploadedBookmarks, options);

      var requestAI = new ProcessBookmarkGroupingRequest()
      {
        Model = "llama3.2",
        Prompt = "With the provided data that is structured in json " + jsonData + " organize the bookmarks by grouping them if applies into folders. Respond using the provided JSON format",
        Stream = false,
        Format = json,
      };

      var responseOllama = await _ollamaService.ProcessBookmarksGroupingAsync(requestAI, cancellationToken);

      // Add to database (store file content table / metrics)

      // Convert response to html file with import format

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

  }
}
