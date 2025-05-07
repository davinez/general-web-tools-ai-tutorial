using System;

namespace CoreApp.API.Features.Bookmarks.Upload;

public class UploadRequest
{
  public required string FileName { get; set; }
  public required byte[] FileContent { get; set; }
  public DateTime UploadTimestamp { get; set; }
}
