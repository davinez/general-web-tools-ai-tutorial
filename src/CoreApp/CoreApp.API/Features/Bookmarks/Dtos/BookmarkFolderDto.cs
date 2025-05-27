using System.Collections.Generic;
using System;

namespace CoreApp.API.Features.Bookmarks.Dtos;

public class BookmarkFolderDto
{
  public required string Title { get; set; }
  public DateTime? AddDate { get; set; }
  public DateTime? LastModified { get; set; }
  public List<BookmarkDto> Bookmarks { get; set; } = [];
  public List<BookmarkFolderDto> SubFolders { get; set; } = [];
}
