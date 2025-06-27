using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Features.Bookmarks.CreateFolders;

public class CreateFoldersRequest
{
  public required List<BookmarkFolderDto> Folders { get; set; }

}
