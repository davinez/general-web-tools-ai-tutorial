namespace CoreApp.API.Domain.Services.JobProviders;

/// <summary>
/// Input parameters when requesting jobs from an <see cref="IJobProvider"/>.
/// </summary>
public record JobProviderRequest
{
  /// <summary>Search keywords (e.g. "Senior .NET Developer").</summary>
  public required string Keywords { get; init; }

  /// <summary>Location filter (e.g. "Mexico City", "Remote").</summary>
  public string Location { get; init; } = string.Empty;

  /// <summary>Maximum number of postings to retrieve per provider call.</summary>
  public int MaxResults { get; init; } = 50;
}
