using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace CoreApp.API.Features.Bookmarks.Dtos;

public class BookmarkFolderDto
{
  public required string Title { get; set; }
  [JsonIgnore]
  public DateTime? AddDate { get; set; }
  public List<BookmarkDto> Bookmarks { get; set; } = [];
  public List<BookmarkFolderDto> SubFolders { get; set; } = [];
}
