using Microsoft.AspNetCore.Http;
using System;

namespace CoreApp.API.Endpoints.JobMarket.Upload;

public record UploadJobScreenshotsRequest
{
  /// <summary>Files to upload (PNG / JPEG / WebP screenshots of job postings).</summary>
  public required IFormFileCollection Files { get; init; }

  /// <summary>ISO-8601 timestamp of when the upload was triggered.</summary>
  public required string UploadTimestamp { get; init; }
}
