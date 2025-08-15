using CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services.ExternalServices;

public interface IAiService
{

  Task<List<CategorizationResponse>> CategorizeIntoFolderNameAsync(string prompt, CancellationToken cancellationToken);

  Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken);
}
