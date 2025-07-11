using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using CoreApp.API.Infrastructure.ExternalServices.Dto;

namespace CoreApp.API.Infrastructure.ExternalServices.S3StorageService;

public interface IS3StorageService
{
  public Task<IEnumerable<FileDto>> GetAllFilesAsync(CancellationToken cancellationToken);
  public Task UploadFileAsync(string bucketName, string objectKey, string contentType, Stream imageBytes, bool enablePayloadSigning);
  public Task<string> DeleteFileAsync(string bucketName, string objectKey);
  public Task<string[]> DeleteFilesAsync(string bucketName, string[] objectKeys);
}
