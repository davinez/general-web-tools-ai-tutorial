using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services;

public interface IAiService
{
    Task<string> GenerateTextAsync(string prompt);
}
