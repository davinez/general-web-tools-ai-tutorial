using System;

namespace CoreApp.API.Infrastructure.MessageBrokers.Dto;

public class UploadBookmarksMessageRequest
{
  public Guid UploadId { get; set; }
  public required string UserId { get; set; }
  public required string HtmlContent { get; set; }
}
