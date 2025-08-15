using CoreApp.API.Domain.Constants;
using System;
using static CoreApp.API.Domain.Constants.StatusConstants;

namespace CoreApp.API.Features.Bookmarks.Upload;

public class UploadResponse
{
  public Guid UploadId { get; set; }
  public bool IsQueuePublishSuccess { get; set; }
  public JobStatus Message { get; set; }
}
