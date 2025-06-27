using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama;

public interface IOllamaService
{
  public Task<ProcessBookmarkGroupingResponse> ProcessBookmarksGroupingAsync(ProcessBookmarkGroupingRequest request, CancellationToken cancellationToken);

}
