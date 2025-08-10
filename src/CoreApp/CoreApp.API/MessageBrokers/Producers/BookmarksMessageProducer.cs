using CoreApp.API.MessageBrokers.Dto;
using CoreApp.API.MessageBrokers.Messages;
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

  public ValueTask PublishUploadBookmarksRequest(UploadBookmarksMessageRequest message)
  {
    return _messageBus.PublishAsync(message);
  }


  // New method to publish DeleteBookmarksMessageRequest
  public ValueTask PublishDeleteRequest(DeleteBookmarksMessageRequest message)
  {
    return _messageBus.PublishAsync(message);
  }

}
