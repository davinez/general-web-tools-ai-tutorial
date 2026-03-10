using CoreApp.API.Domain.Services.JobProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.JobProviders.Providers;

/// <summary>
/// Placeholder <see cref="IJobProvider"/> for LinkedIn.
///
/// TODO (Phase 1→2): Replace the stub HTTP call with the real LinkedIn
///      Job Search API once the API key is provisioned in Azure Key Vault.
///      Docs: https://learn.microsoft.com/en-us/linkedin/shared/integrations/jobs
/// </summary>
public class LinkedInJobProvider : IJobProvider
{
  public string ProviderName => "LinkedIn";

  private readonly HttpClient _httpClient;
  private readonly ILogger<LinkedInJobProvider> _logger;
  private readonly string _apiKey;
  private readonly int _maxResults;

  public LinkedInJobProvider(
      HttpClient httpClient,
      IConfiguration configuration,
      ILogger<LinkedInJobProvider> logger)
  {
    _httpClient = httpClient;
    _logger = logger;
    _apiKey = configuration["JobProviders:LinkedIn:ApiKey"] ?? string.Empty;
    _maxResults = int.TryParse(configuration["JobProviders:LinkedIn:MaxResultsPerRun"], out var max)
        ? max
        : 50;
  }

  public Task<IReadOnlyList<RawJobPostingDto>> FetchJobsAsync(
      JobProviderRequest request,
      CancellationToken cancellationToken)
  {
    // -----------------------------------------------------------------------
    // TODO: Implement the real LinkedIn API call here.
    //       The stub below returns an empty list so the pipeline can be
    //       exercised end-to-end before the API key is available.
    // -----------------------------------------------------------------------

    if (string.IsNullOrWhiteSpace(_apiKey))
    {
      _logger.LogWarning(
          "[{Provider}] No API key configured. Returning empty result. " +
          "Set JobProviders:LinkedIn:ApiKey in appsettings or Key Vault.",
          ProviderName);

      return Task.FromResult<IReadOnlyList<RawJobPostingDto>>([]);
    }

    // Real implementation skeleton (uncomment & flesh out when key is ready):
    // _httpClient.DefaultRequestHeaders.Authorization =
    //     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
    //
    // var response = await _httpClient.GetFromJsonAsync<LinkedInApiResponse>(
    //     $"jobs/search?keywords={Uri.EscapeDataString(request.Keywords)}" +
    //     $"&location={Uri.EscapeDataString(request.Location)}" +
    //     $"&count={Math.Min(request.MaxResults, _maxResults)}",
    //     cancellationToken);
    //
    // return response?.Elements.Select(e => new RawJobPostingDto
    // {
    //     ExternalId = e.EntityUrn,
    //     Title      = e.Title,
    //     Company    = e.CompanyName,
    //     Location   = e.FormattedLocation,
    //     PostedAt   = e.PostedAt,
    //     SourceProvider = ProviderName,
    // }).ToList() ?? [];

    _logger.LogInformation(
        "[{Provider}] Stub provider returning 0 jobs (API key not yet configured).",
        ProviderName);

    return Task.FromResult<IReadOnlyList<RawJobPostingDto>>([]);
  }
}
