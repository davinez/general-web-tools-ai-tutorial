using System;
using Microsoft.AspNetCore.Http;

namespace CoreApp.API.Endpoints.Bookmarks.Upload;

public class UploadRequest
{
  public required string FileName { get; set; }
  public required IFormFile FileContent { get; set; }
  public DateTimeOffset UploadTimestamp { get; set; }
}
