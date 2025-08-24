using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using System.Threading.Tasks;
using Wolverine;

namespace CoreApp.API.Infrastructure.MessageBrokers.Producers;

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
