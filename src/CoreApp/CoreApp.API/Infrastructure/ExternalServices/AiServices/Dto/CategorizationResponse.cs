namespace CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;

/// <summary>
/// DTO for the AI's response in Phase 1, UploadBookmarksMessageRequest
/// </summary>
public class CategorizationResponse
{
  public int Id { get; set; }
  public required string FolderName { get; set; }
}
