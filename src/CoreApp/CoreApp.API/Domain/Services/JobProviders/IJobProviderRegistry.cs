using System.Collections.Generic;

namespace CoreApp.API.Domain.Services.JobProviders;

/// <summary>
/// Holds all registered <see cref="IJobProvider"/> instances and exposes
/// retrieval by name or as a full list.
/// Implementations are registered as Singletons in DI.
/// </summary>
public interface IJobProviderRegistry
{
  /// <summary>Returns the provider with the given name, or null if not found.</summary>
  IJobProvider? GetProvider(string name);

  /// <summary>Returns all registered providers.</summary>
  IReadOnlyList<IJobProvider> GetAll();
}
