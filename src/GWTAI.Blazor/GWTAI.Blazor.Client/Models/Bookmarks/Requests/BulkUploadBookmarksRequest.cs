namespace GWTAI.Blazor.Client.Models.Bookmarks.Requests;

public class BulkUploadBookmarksRequest
{
  public required string FileName { get; set; }
  public required byte[] FileContent { get; set; }
  public DateTime UploadTimestamp { get; set; }
  // public string? AdditionalProperty { get; set; } // Optional property for future use
}
