using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;

public class ProcessBookmarkGroupingRequest
{
  public string Model { get; set; } = "llama3.2";
  public string Prompt { get; set; } = "Organize the following bookmarks into folders. Respond using JSON";
  public bool Stream { get; set; } = false;
  public string Format { get; set; } = string.Empty;

  //public required List<BookmarkDto> Bookmarks { get; set; }
}


