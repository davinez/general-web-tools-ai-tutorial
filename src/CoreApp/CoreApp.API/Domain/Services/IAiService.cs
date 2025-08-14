using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services;

public interface IAiService
{
    Task<string> GenerateTextAsync(string prompt);
    Task<BookmarkGroupingResponse> BookmarksGroupingAsync(BookmarkGroupingRequest request, CancellationToken cancellationToken);
}
