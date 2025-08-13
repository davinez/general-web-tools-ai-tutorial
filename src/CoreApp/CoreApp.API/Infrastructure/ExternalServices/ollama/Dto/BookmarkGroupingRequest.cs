using System.Collections.Generic;
using System.Text.Json;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;

public class BookmarkGroupingRequest
{
  public string Model { get; set; } = "llama3.2";
  public string Prompt { get; set; } = "Organize the following bookmarks into folders. Respond using JSON";
  public bool Stream { get; set; } = false;
  public JsonElement Format { get; set; }
}


