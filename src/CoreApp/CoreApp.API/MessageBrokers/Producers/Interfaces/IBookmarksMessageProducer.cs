using CoreApp.API.MessageBrokers.Dto;
using CoreApp.API.MessageBrokers.Messages;
using System.Threading.Tasks;

namespace CoreApp.API.MessageBrokers.Producers.Interfaces;

public interface IBookmarksMessageProducer
{
  public ValueTask PublishUploadBookmarksRequest(UploadBookmarksMessageRequest message);

  public ValueTask PublishDeleteRequest(DeleteBookmarksMessageRequest message);
}
