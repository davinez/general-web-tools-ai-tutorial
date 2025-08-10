using System;

namespace CoreApp.API.MessageBrokers.Dto;

public class DeleteBookmarksMessageRequest
{
  public Guid BookmarkId { get; set; }
}

