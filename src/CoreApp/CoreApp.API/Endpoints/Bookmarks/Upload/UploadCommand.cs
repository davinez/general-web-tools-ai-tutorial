using CoreApp.API.Domain;
using CoreApp.API.Domain.MessageBrokers.Producers;
using CoreApp.API.Domain.Security;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static CoreApp.API.Domain.Constants.StatusConstants;

namespace CoreApp.API.Endpoints.Bookmarks.Upload;

public record UploadCommand(UploadRequest File) : ICommand<UploadResponse>;

public class UploadCommandHandler
{
  public class CommandValidator : AbstractValidator<UploadCommand>
  {
    public CommandValidator()
    {
      RuleFor(x => x.File).NotNull();

      RuleFor(x => x.File.FileName).NotNull().NotEmpty();
      // TODO: check type of file
      RuleFor(x => x.File.FileContent).NotNull().NotEmpty();
      RuleFor(x => x.File.UploadTimestamp).NotNull().NotEmpty();


    }
  }

  public class Handler : ICommandHandler<UploadCommand, UploadResponse>
  {

    private readonly ILogger<UploadCommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IBookmarksMessageProducer _bookmarksMessageProducer;
    private readonly CoreAppContext _context;
    private readonly JsonSerializerOptions _options;

    public Handler(
      ILogger<UploadCommandHandler> logger,
      ICurrentUserAccessor currentUserAccessor,
      IBookmarksMessageProducer bookmarksMessageProducer,
      CoreAppContext context,
      JsonSerializerOptions options)
    {
      _logger = logger;
      _currentUserAccessor = currentUserAccessor;
      _bookmarksMessageProducer = bookmarksMessageProducer;
      _context = context;
      _options = options;
    }


    public async ValueTask<UploadResponse> Handle(
        UploadCommand command,
        CancellationToken cancellationToken
    )
    {
      // Generate a unique ID for this upload operation
      Guid uploadId = Guid.NewGuid();

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
        UserId = _currentUserAccessor.GetCurrentUsername(),
        HtmlContent = cleanedHtml
      };

      try
      {
        await _bookmarksMessageProducer.PublishUploadBookmarksRequest(uploadRequestedMessage);

        _logger.LogInformation($"Upload command processed successfully with uploadId {uploadId}");

        var newJobEvent = JobEvent.Create(
          uploadId,
          _currentUserAccessor.GetCurrentUsername(),
          JobStatus.InProgress,
          JsonSerializer.Serialize(new { Title = command.File.FileName }, _options),
          Workflow.BookmarksUpload
          );

        _context.JobEvents.Add(newJobEvent);

        await _context.SaveChangesAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error ocurred in {nameof(UploadCommandHandler)}: with message {ex.Message} and request data: uploadId {uploadId}");

        var newJobEvent = JobEvent.Create(
                          uploadId,
                          _currentUserAccessor.GetCurrentUsername(),
                          JobStatus.Failed,
                          JsonSerializer.Serialize(new { Title = command.File.FileName }, _options),
                          Workflow.BookmarksUpload
                          );
        _context.JobEvents.Add(newJobEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return new UploadResponse() { UploadId = uploadId, IsQueuePublishSuccess = false, Message = JobStatus.Failed.ToString() };
      }

      return new UploadResponse() { UploadId = uploadId, IsQueuePublishSuccess = true, Message = JobStatus.InProgress.ToString() };
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
