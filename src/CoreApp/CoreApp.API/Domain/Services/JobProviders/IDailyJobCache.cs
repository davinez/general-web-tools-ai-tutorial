using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services.JobProviders;

/// <summary>
/// Daily-lock cache that prevents a provider from being called more than once
/// within a calendar day (UTC).
/// Cache key format: <c>daily-lock:{providerName}:{yyyyMMdd}</c>
/// TTL: End of the current UTC calendar day.
/// </summary>
public interface IDailyJobCache
{
  /// <summary>Returns true if the provider has already been called today.</summary>
  Task<bool> HasRunTodayAsync(string providerName, CancellationToken cancellationToken);

  /// <summary>Records that the provider ran today and caches its results.</summary>
  Task MarkAsRunAsync(
      string providerName,
      IReadOnlyList<RawJobPostingDto> jobs,
      CancellationToken cancellationToken);

  /// <summary>
  /// Returns the cached results for today, or null if the cache has no entry.
  /// </summary>
  Task<IReadOnlyList<RawJobPostingDto>?> GetResultAsync(
      string providerName,
      CancellationToken cancellationToken);
}
