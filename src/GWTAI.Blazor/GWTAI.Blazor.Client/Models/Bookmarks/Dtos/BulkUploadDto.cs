namespace GWTAI.Blazor.Client.Models.Bookmarks.Dtos;

public class BulkUploadDto
{
  public required string FileName { get; set; }
  public required byte[] FileContent { get; set; } 

}
