using CoreApp.API.Domain.Services.JobProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApp.API.Infrastructure.ExternalServices.JobProviders;

/// <summary>
/// Singleton registry that holds all <see cref="IJobProvider"/> instances
/// injected via DI (<c>IEnumerable&lt;IJobProvider&gt;</c>).
/// </summary>
public class JobProviderRegistry : IJobProviderRegistry
{
  private readonly IReadOnlyDictionary<string, IJobProvider> _providers;

  public JobProviderRegistry(IEnumerable<IJobProvider> providers)
  {
    _providers = providers.ToDictionary(
        p => p.ProviderName,
        p => p,
        StringComparer.OrdinalIgnoreCase);
  }

  public IJobProvider? GetProvider(string name) =>
      _providers.TryGetValue(name, out var provider) ? provider : null;

  public IReadOnlyList<IJobProvider> GetAll() =>
      _providers.Values.ToList().AsReadOnly();
}
