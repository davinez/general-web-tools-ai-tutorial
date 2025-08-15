using System.IO;

namespace CoreApp.API.Infrastructure.ExternalServices.Storage.Dto;

public class FileDto
{
  public required string FileName { get; set; }
  public required Stream Content { get; set; }
  public required string ContentType { get; set; }
}
