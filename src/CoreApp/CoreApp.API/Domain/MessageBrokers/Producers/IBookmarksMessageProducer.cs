using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.MessageBrokers.Producers;

public interface IBookmarksMessageProducer
{
  public ValueTask PublishUploadBookmarksRequest(UploadBookmarksMessageRequest message);

  public ValueTask PublishDeleteRequest(DeleteBookmarksMessageRequest message);
}
