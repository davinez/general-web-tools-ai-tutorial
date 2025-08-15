using Amazon.S3;
using Amazon.S3.Model;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.Storage;

public class S3StorageService : IStorageService
{
  private readonly IConfiguration _configuration;

  public S3StorageService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public async Task<string> UploadFileAsync(FileDto file)
  {
    var bucketName = _configuration["StorageService:S3:BucketName"];
    using var s3Client = GenerateS3Client(bucketName);

    var request = new PutObjectRequest
    {
      BucketName = bucketName,
      Key = file.FileName,
      InputStream = file.Content,
      ContentType = file.ContentType
    };

    await s3Client.PutObjectAsync(request);

    return $"https://{bucketName}.s3.amazonaws.com/{file.FileName}";
  }

  public async Task DeleteFileAsync(string fileUrl)
  {
    var bucketName = _configuration["StorageService:S3:BucketName"];
    using var s3Client = GenerateS3Client(bucketName);

    var key = new Uri(fileUrl).Segments.Last();

    var request = new DeleteObjectRequest
    {
      BucketName = bucketName,
      Key = key
    };

    await s3Client.DeleteObjectAsync(request);
  }

  private AmazonS3Client GenerateS3Client(string bucketName)
  {
    var config = new AmazonS3Config
    {
      ServiceURL = _configuration["StorageService:S3:ServiceUrl"]
    };

    var accessKey = _configuration["StorageService:S3:AccessKey"];
    var secretKey = _configuration["StorageService:S3:SecretKey"];

    return new AmazonS3Client(accessKey, secretKey, config);
  }
}
