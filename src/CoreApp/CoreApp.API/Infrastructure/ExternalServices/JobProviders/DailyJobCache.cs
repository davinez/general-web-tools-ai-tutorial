using CoreApp.API.Domain.Services.JobProviders;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.JobProviders;

/// <summary>
/// Daily-lock cache backed by <see cref="IDistributedCache"/> (Redis in prod,
/// in-memory distributed cache in dev if Redis is unavailable).
///
/// Key format : <c>daily-lock:{providerName}:{yyyyMMdd}</c>
/// TTL        : Seconds remaining until midnight UTC today (so the lock expires
///              naturally at the start of the next calendar day).
/// </summary>
public class DailyJobCache : IDailyJobCache
{
  private readonly IDistributedCache _cache;
  private readonly ILogger<DailyJobCache> _logger;

  public DailyJobCache(IDistributedCache cache, ILogger<DailyJobCache> logger)
  {
    _cache = cache;
    _logger = logger;
  }

  public async Task<bool> HasRunTodayAsync(string providerName, CancellationToken cancellationToken)
  {
    var key = LockKey(providerName);
    var value = await _cache.GetStringAsync(key, cancellationToken);
    return value is not null;
  }

  public async Task MarkAsRunAsync(
      string providerName,
      IReadOnlyList<RawJobPostingDto> jobs,
      CancellationToken cancellationToken)
  {
    var key = LockKey(providerName);
    var resultsKey = ResultsKey(providerName);
    var options = BuildOptions();

    // Store the lock sentinel
    await _cache.SetStringAsync(key, "1", options, cancellationToken);

    // Store the serialized results
    var json = JsonSerializer.Serialize(jobs);
    await _cache.SetStringAsync(resultsKey, json, options, cancellationToken);

    _logger.LogInformation(
        "Daily-lock set for provider {Provider}. {Count} job(s) cached until {MidnightUtc:O}.",
        providerName, jobs.Count, MidnightUtc());
  }

  public async Task<IReadOnlyList<RawJobPostingDto>?> GetResultAsync(
      string providerName,
      CancellationToken cancellationToken)
  {
    var json = await _cache.GetStringAsync(ResultsKey(providerName), cancellationToken);
    if (json is null)
    {
      return null;
    }

    return JsonSerializer.Deserialize<List<RawJobPostingDto>>(json);
  }

  // ── Helpers ──────────────────────────────────────────────────────────────

  private static string DateStamp() => DateTime.UtcNow.ToString("yyyyMMdd");

  private static string LockKey(string provider) =>
      $"daily-lock:{provider}:{DateStamp()}";

  private static string ResultsKey(string provider) =>
      $"daily-results:{provider}:{DateStamp()}";

  private static DateTime MidnightUtc() =>
      DateTime.UtcNow.Date.AddDays(1); // Next midnight UTC

  private static DistributedCacheEntryOptions BuildOptions()
  {
    var secondsUntilMidnight = (MidnightUtc() - DateTime.UtcNow).TotalSeconds;

    return new DistributedCacheEntryOptions
    {
      // Absolute expiry at midnight keeps the lock tightly scoped to a calendar day
      AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(secondsUntilMidnight),
    };
  }
}
