using CoreApp.API.MessageBrokers.Dto;
using CoreApp.API.MessageBrokers.Messages;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Wolverine;

namespace CoreApp.API.MessageBrokers.Producers;

public class BookmarksMessageProducer
{
  private readonly IMessageBus _messageBus;

  public BookmarksMessageProducer(IMessageBus messageBus)
  {
    _messageBus = messageBus;
  }

  public ValueTask PublishUploadRequest(UploadBookmarksMessageRequest message)
  {
    return _messageBus.PublishAsync(message);
  }

  // Message type 2
  // Add it in a separate file
  public record DeleteBookmarksMessageRequest(Guid UserId, IEnumerable<Guid> BookmarkIds);

  // New method to publish DeleteBookmarksMessageRequest
  public ValueTask PublishDeleteRequest(DeleteBookmarksMessageRequest message)
  {
    return _messageBus.PublishAsync(message);
  }

}
