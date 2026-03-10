using CoreApp.API.Domain;
using CoreApp.API.Domain.Security;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Domain.Services.JobProviders;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static CoreApp.API.Domain.Constants.StatusConstants;

namespace CoreApp.API.Endpoints.JobMarket.FetchJobs;

public record FetchJobsCommand(FetchJobsRequest Request) : ICommand<FetchJobsResponse>;

public record FetchJobsRequest
{
  public required string Keywords { get; init; }
  public string Location { get; init; } = string.Empty;
  public int MaxResults { get; init; } = 50;
}

public class FetchJobsCommandHandler
{
  public class CommandValidator : AbstractValidator<FetchJobsCommand>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Request.Keywords)
          .NotNull()
          .NotEmpty()
          .WithMessage("Keywords are required to search for jobs.");

      RuleFor(x => x.Request.MaxResults)
          .InclusiveBetween(1, 200)
          .WithMessage("MaxResults must be between 1 and 200.");
    }
  }

  public class Handler : ICommandHandler<FetchJobsCommand, FetchJobsResponse>
  {
    private readonly ILogger<FetchJobsCommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IJobProviderRegistry _registry;
    private readonly IDailyJobCache _dailyJobCache;
    private readonly IStorageService _storageService;
    private readonly CoreAppContext _context;

    public Handler(
        ILogger<FetchJobsCommandHandler> logger,
        ICurrentUserAccessor currentUserAccessor,
        IJobProviderRegistry registry,
        IDailyJobCache dailyJobCache,
        IStorageService storageService,
        CoreAppContext context)
    {
      _logger = logger;
      _currentUserAccessor = currentUserAccessor;
      _registry = registry;
      _dailyJobCache = dailyJobCache;
      _storageService = storageService;
      _context = context;
    }

    public async ValueTask<FetchJobsResponse> Handle(
        FetchJobsCommand command,
        CancellationToken cancellationToken)
    {
      var jobEventId = Guid.NewGuid();
      var userId = _currentUserAccessor.GetCurrentUsername();
      var datePath = DateTime.UtcNow.ToString("yyyy/MM/dd");
      var summaries = new List<ProviderFetchSummary>();

      var providerRequest = new JobProviderRequest
      {
        Keywords = command.Request.Keywords,
        Location = command.Request.Location,
        MaxResults = command.Request.MaxResults,
      };

      // Loop every registered provider; apply daily-lock guard per provider
      foreach (var provider in _registry.GetAll())
      {
        if (await _dailyJobCache.HasRunTodayAsync(provider.ProviderName, cancellationToken))
        {
          _logger.LogInformation(
              "[{Provider}] Already ran today – skipping (daily-lock active).",
              provider.ProviderName);

          summaries.Add(new ProviderFetchSummary
          {
            ProviderName = provider.ProviderName,
            Skipped = true,
            JobsFetched = 0,
          });
          continue;
        }

        try
        {
          var jobs = await provider.FetchJobsAsync(providerRequest, cancellationToken);

          // Persist daily-lock and cache results
          await _dailyJobCache.MarkAsRunAsync(provider.ProviderName, jobs, cancellationToken);

          // Upload raw JSON to the Bronze Blob layer
          string? blobPath = null;
          if (jobs.Count > 0)
          {
            var json = JsonSerializer.Serialize(jobs, new JsonSerializerOptions
            {
              WriteIndented = false,
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            blobPath = $"bronze/jobs/{datePath}/{provider.ProviderName}/{jobEventId}.json";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await _storageService.UploadFileAsync(new FileDto
            {
              FileName = blobPath,
              Content = stream,
              ContentType = "application/json",
            });

            _logger.LogInformation(
                "[{Provider}] Saved {Count} job(s) to Bronze layer at {BlobPath}.",
                provider.ProviderName, jobs.Count, blobPath);
          }

          summaries.Add(new ProviderFetchSummary
          {
            ProviderName = provider.ProviderName,
            Skipped = false,
            JobsFetched = jobs.Count,
            BlobPath = blobPath,
          });
        }
        catch (Exception ex)
        {
          _logger.LogError(ex,
              "[{Provider}] Error during FetchJobsAsync.",
              provider.ProviderName);

          summaries.Add(new ProviderFetchSummary
          {
            ProviderName = provider.ProviderName,
            Skipped = false,
            JobsFetched = 0,
          });

          _context.JobEvents.Add(JobEvent.Create(
              jobEventId,
              userId,
              JobStatus.Failed,
              new { Keywords = command.Request.Keywords, Providers = summaries },
              Workflow.JobFetch));

          await _context.SaveChangesAsync(cancellationToken);
        }
      }

      // Persist a JobEvent tracking the overall fetch run
      var jobEvent = JobEvent.Create(
          jobEventId,
          userId,
          JobStatus.Complete,
          new { Keywords = command.Request.Keywords, Providers = summaries },
          Workflow.JobFetch);

      _context.JobEvents.Add(jobEvent);
      await _context.SaveChangesAsync(cancellationToken);

      return new FetchJobsResponse
      {
        Message = JobStatus.InProgress.ToString(),
        ProviderSummaries = summaries,
      };
    }
  }
}
