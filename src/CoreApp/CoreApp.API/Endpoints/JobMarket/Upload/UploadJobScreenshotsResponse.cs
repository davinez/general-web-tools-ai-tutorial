using System;
using System.Collections.Generic;

namespace CoreApp.API.Endpoints.JobMarket.Upload;

public record UploadJobScreenshotsResponse
{
  public Guid JobEventId { get; init; }

  /// <summary>Blob URLs of the uploaded screenshots in the Bronze layer.</summary>
  public IReadOnlyList<string> BlobUrls { get; init; } = [];

  public string Message { get; init; } = string.Empty;
}
