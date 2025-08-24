using System;

namespace CoreApp.API.Infrastructure.MessageBrokers.Dto;

public class DeleteBookmarksMessageRequest
{
  public Guid BookmarkId { get; set; }
}

