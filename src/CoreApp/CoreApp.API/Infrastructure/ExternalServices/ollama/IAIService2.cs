using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama;

public interface IAIService2
{
  public Task<BookmarkGroupingResponse> BookmarksGroupingAsync(BookmarkGroupingRequest request, CancellationToken cancellationToken);

}
