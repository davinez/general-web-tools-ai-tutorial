using System;

namespace CoreApp.API.MessageBrokers.Dto;

public class UploadBookmarksMessageRequest
{
  public Guid UploadId { get; set; }
  public required string HtmlContent { get; set; }
}
