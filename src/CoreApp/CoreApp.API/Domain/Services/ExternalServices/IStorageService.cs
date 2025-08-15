using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Services.ExternalServices
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(FileDto file);
        Task DeleteFileAsync(string fileUrl);
    }
}
