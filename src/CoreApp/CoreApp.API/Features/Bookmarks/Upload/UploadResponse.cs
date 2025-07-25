using System;

namespace CoreApp.API.Features.Bookmarks.Upload;

public class UploadResponse
{
  public Guid UploadId { get; set; }
  public bool IsQueuePublishSuccess { get; set; }
  public string? Message { get; set; }
}
