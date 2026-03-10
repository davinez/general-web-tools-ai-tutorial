using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services.JobProviders;

/// <summary>
/// Abstraction for an external job-posting data source.
/// Each implementation encapsulates the API call, authentication, and response mapping
/// for a specific provider (e.g. LinkedIn, Indeed, Adzuna).
/// </summary>
public interface IJobProvider
{
  /// <summary>Unique identifier for this provider (e.g. "LinkedIn").</summary>
  string ProviderName { get; }

  /// <summary>Fetches raw job postings from the external source.</summary>
  Task<IReadOnlyList<RawJobPostingDto>> FetchJobsAsync(
      JobProviderRequest request,
      CancellationToken cancellationToken);
}
