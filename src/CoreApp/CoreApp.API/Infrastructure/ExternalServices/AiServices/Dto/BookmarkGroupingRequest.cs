using System.Text.Json;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;

public class BookmarkGroupingRequest
{
  public string Model { get; set; } = "llama3.2";
  public string Prompt { get; set; } = "Organize the following bookmarks into folders. Respond using JSON";
  public bool Stream { get; set; } = false;
  public JsonElement Format { get; set; }
}


