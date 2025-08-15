using CoreApp.API.Domain;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.MessageBrokers.Dto;
using CoreApp.API.MessageBrokers.Producers.Interfaces;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static CoreApp.API.Domain.Constants.StatusConstants;

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

    private readonly ILogger<UploadCommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IBookmarksMessageProducer _bookmarksMessageProducer;
    private readonly CoreAppContext _context;

    public Handler(
      ILogger<UploadCommandHandler> logger,
      ICurrentUserAccessor currentUserAccessor,
      IBookmarksMessageProducer bookmarksMessageProducer,
      CoreAppContext context)
    {
      _logger = logger;
      _currentUserAccessor = currentUserAccessor;
      _bookmarksMessageProducer = bookmarksMessageProducer;
      _context = context;
    }


    public async ValueTask<UploadResponse> Handle(
        UploadCommand command,
        CancellationToken cancellationToken
    )
    {
      // Generate a unique ID for this upload operation
      var uploadId = Guid.NewGuid();

      // TODO: Move all logic to a separate service and the endpoint will send a event and return in progress, the service
      // will process and update the event with fail with reason or success with result

      // Use mass transit alternative, Wolverine https://wolverinefx.net/guide/messaging/message-bus.html
      // https://medium.com/@ms111mithun/mastering-message-queues-leveraging-rabbitmq-locally-and-azure-service-bus-in-production-for-net-277236f25609

      // Where send the html file generated with the grouped bookmarks?


      // Read the file content into a byte array (if needed)
      using var memoryStream = new MemoryStream();
      await command.File.FileContent.CopyToAsync(memoryStream, cancellationToken);
      var fileBytes = memoryStream.ToArray();

      string htmlContentString = System.Text.Encoding.UTF8.GetString(fileBytes);

      // Remove all <p> nodes
      string cleanedHtml = CleanHtml(htmlContentString);

      // Create the message to be published
      var uploadRequestedMessage = new UploadBookmarksMessageRequest
      {
        UploadId = uploadId,
        HtmlContent = cleanedHtml
      };

      try
      {
        await _bookmarksMessageProducer.PublishUploadBookmarksRequest(uploadRequestedMessage);

        _logger.LogInformation($"Upload command processed successfully with uploadId {uploadId}");

        var newJobEvent = new JobEvent
        {
          JobEventId = uploadId,
          UserId = "test", //_currentUserAccessor.UserId,
          Status = JobStatus.InProgress.ToString(),
          Content = JsonSerializer.Serialize(new { Title = command.File.FileName}, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
          EventTimestamp = DateTime.UtcNow,
          Workflow = Workflow.BookmarksUpload.ToString()
        };

        _context.JobEvents.Add(newJobEvent);
        await _context.SaveChangesAsync(cancellationToken);

      }
      catch (Exception ex)
      {
        _logger.LogError($"Error ocurred in {nameof(UploadCommandHandler)}: with message {ex.Message} and request data: uploadId {uploadId}");

        return new UploadResponse() { UploadId = uploadId, IsQueuePublishSuccess = false, Message = JobStatus.Failed };
      }

      return new UploadResponse() { UploadId = uploadId, IsQueuePublishSuccess = true, Message = JobStatus.InProgress };
    }


    public static string CleanHtml(string htmlContent)
    {

      return htmlContent.Replace("<p>", string.Empty)
                        .Replace("<P>", string.Empty)
                        .Replace("</P>", string.Empty)
                        .Replace("</p>", string.Empty);

    }

  }
}
