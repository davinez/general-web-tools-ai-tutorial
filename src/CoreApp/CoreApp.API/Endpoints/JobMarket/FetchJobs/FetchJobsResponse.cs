using System.Collections.Generic;

namespace CoreApp.API.Endpoints.JobMarket.FetchJobs;

public record FetchJobsResponse
{
  public string Message { get; init; } = string.Empty;

  /// <summary>Per-provider summary: how many jobs were fetched (0 = skipped due to daily lock).</summary>
  public IReadOnlyList<ProviderFetchSummary> ProviderSummaries { get; init; } = [];
}

public record ProviderFetchSummary
{
  public string ProviderName { get; init; } = string.Empty;
  public bool Skipped { get; init; }
  public int JobsFetched { get; init; }
  public string? BlobPath { get; init; }
}
