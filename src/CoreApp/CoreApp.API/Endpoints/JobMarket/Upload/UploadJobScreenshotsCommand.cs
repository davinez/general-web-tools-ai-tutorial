using CoreApp.API.Domain;
using CoreApp.API.Domain.Constants;
using CoreApp.API.Domain.Security;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static CoreApp.API.Domain.Constants.StatusConstants;

namespace CoreApp.API.Endpoints.JobMarket.Upload;

public record UploadJobScreenshotsCommand(UploadJobScreenshotsRequest Request) : ICommand<UploadJobScreenshotsResponse>;

public class UploadJobScreenshotsCommandHandler
{
  private static readonly HashSet<string> _allowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
  {
    "image/png",
    "image/jpeg",
    "image/jpg",
    "image/webp",
  };

  // ── Validator ────────────────────────────────────────────────────────────
  public class CommandValidator : AbstractValidator<UploadJobScreenshotsCommand>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Request).NotNull();

      RuleFor(x => x.Request.Files)
          .NotNull()
          .Must(files => files.Count > 0)
          .WithMessage("At least one screenshot file is required.");

      RuleFor(x => x.Request.UploadTimestamp)
          .NotNull()
          .NotEmpty()
          .WithMessage("UploadTimestamp is required.");

      RuleForEach(x => x.Request.Files).ChildRules(file =>
      {
        file.RuleFor(f => f.ContentType)
            .Must(ct => _allowedContentTypes.Contains(ct))
            .WithMessage(f => $"File '{f.FileName}' has unsupported content type '{f.ContentType}'. Allowed: png, jpeg, webp.");

        file.RuleFor(f => f.Length)
            .LessThanOrEqualTo(20 * 1024 * 1024) // 20 MB per file
            .WithMessage(f => $"File '{f.FileName}' exceeds the 20 MB size limit.");
      });
    }
  }

  // ── Handler ──────────────────────────────────────────────────────────────
  public class Handler : ICommandHandler<UploadJobScreenshotsCommand, UploadJobScreenshotsResponse>
  {
    private readonly ILogger<UploadJobScreenshotsCommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IStorageService _storageService;
    private readonly CoreAppContext _context;

    public Handler(
        ILogger<UploadJobScreenshotsCommandHandler> logger,
        ICurrentUserAccessor currentUserAccessor,
        IStorageService storageService,
        CoreAppContext context)
    {
      _logger = logger;
      _currentUserAccessor = currentUserAccessor;
      _storageService = storageService;
      _context = context;
    }

    public async ValueTask<UploadJobScreenshotsResponse> Handle(
        UploadJobScreenshotsCommand command,
        CancellationToken cancellationToken)
    {
      var jobEventId = Guid.NewGuid();
      var userId = _currentUserAccessor.GetCurrentUsername();
      var datePath = DateTime.UtcNow.ToString("yyyy/MM/dd");
      var blobUrls = new List<string>(command.Request.Files.Count);

      try
      {
        // Upload each screenshot to the Bronze layer
        foreach (var formFile in command.Request.Files)
        {
          var safeFileName = Path.GetFileName(formFile.FileName);
          var blobName = $"bronze/screenshots/{datePath}/{jobEventId}_{safeFileName}";

          using var stream = formFile.OpenReadStream();
          var fileDto = new FileDto
          {
            FileName = blobName,
            Content = stream,
            ContentType = formFile.ContentType,
          };

          var blobUrl = await _storageService.UploadFileAsync(fileDto);
          blobUrls.Add(blobUrl);

          _logger.LogInformation(
              "Uploaded screenshot {FileName} to Bronze layer at {BlobUrl}",
              safeFileName, blobUrl);
        }

        // Persist a JobEvent so the UI / SignalR hub can track progress
        var jobEvent = JobEvent.Create(
            jobEventId,
            userId,
            JobStatus.InProgress,
            new { FileCount = blobUrls.Count, BlobUrls = blobUrls },
            Workflow.ScreenshotsUpload);

        _context.JobEvents.Add(jobEvent);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "UploadJobScreenshots completed. JobEventId={JobEventId}, Files={FileCount}",
            jobEventId, blobUrls.Count);

        return new UploadJobScreenshotsResponse
        {
          JobEventId = jobEventId,
          BlobUrls = blobUrls,
          Message = JobStatus.InProgress.ToString(),
        };
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
            "Error in {Handler}: JobEventId={JobEventId}",
            nameof(UploadJobScreenshotsCommandHandler), jobEventId);

        var failedEvent = JobEvent.Create(
            jobEventId,
            userId,
            JobStatus.Failed,
            new { Error = ex.Message },
            Workflow.ScreenshotsUpload);

        _context.JobEvents.Add(failedEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return new UploadJobScreenshotsResponse
        {
          JobEventId = jobEventId,
          BlobUrls = [],
          Message = JobStatus.Failed.ToString(),
        };
      }
    }
  }
}
