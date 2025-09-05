using Azure.Storage.Blobs;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.Storage
{
  public class AzureBlobStorageService : IStorageService
  {
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public AzureBlobStorageService(IConfiguration configuration)
    {
      _configuration = configuration;
      var connectionString = _configuration["StorageService:AzureBlobStorage:ConnectionString"];
      _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadFileAsync(FileDto file)
    {
      var containerName = _configuration["StorageService:AzureBlobStorage:ContainerName"];
      var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
      await containerClient.CreateIfNotExistsAsync();

      var blobClient = containerClient.GetBlobClient(file.FileName);
      await blobClient.UploadAsync(file.Content, true);

      return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
      var containerName = _configuration["StorageService:AzureBlobStorage:ContainerName"];
      var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

      var blobName = new Uri(fileUrl).Segments[^1];
      var blobClient = containerClient.GetBlobClient(blobName);

      await blobClient.DeleteIfExistsAsync();
    }
  }
}
