using CoreApp.API.Infrastructure.ExternalServices.Dto;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(FileDto file);
        Task DeleteFileAsync(string fileUrl);
    }
}
