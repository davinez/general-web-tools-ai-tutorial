using System;

namespace CoreApp.API.MessageBrokers.Messages;

public class UploadBookmarksMessageRequest
{
  public Guid UploadId { get; set; }
  public required string HtmlContent { get; set; }
}
