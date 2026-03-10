using System;

namespace CoreApp.API.Domain.Services.JobProviders;

/// <summary>
/// Raw, un-normalized job posting as returned by an external provider.
/// This is what gets stored in the Bronze Blob layer as JSON.
/// The FastAPI Processing service is responsible for normalizing it into the Silver layer.
/// </summary>
public record RawJobPostingDto
{
  /// <summary>Provider-specific unique identifier for the posting.</summary>
  public required string ExternalId { get; init; }

  public required string Title { get; init; }

  public required string Company { get; init; }

  public string Location { get; init; } = string.Empty;

  /// <summary>Raw HTML or plain-text description from the provider.</summary>
  public string DescriptionHtml { get; init; } = string.Empty;

  public DateTimeOffset? PostedAt { get; init; }

  /// <summary>Name of the <see cref="IJobProvider"/> that produced this record.</summary>
  public required string SourceProvider { get; init; }
}
