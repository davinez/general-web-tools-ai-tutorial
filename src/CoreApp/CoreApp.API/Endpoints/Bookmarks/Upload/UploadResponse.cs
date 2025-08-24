using System;

namespace CoreApp.API.Endpoints.Bookmarks.Upload;

public class UploadResponse
{
  public Guid UploadId { get; set; }
  public bool IsQueuePublishSuccess { get; set; }
  public required string Message { get; set; }
}
