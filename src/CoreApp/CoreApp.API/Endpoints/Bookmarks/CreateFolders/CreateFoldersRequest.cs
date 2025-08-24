using System.Collections.Generic;
using CoreApp.API.Endpoints.Bookmarks.Dtos;

namespace CoreApp.API.Endpoints.Bookmarks.CreateFolders;

public class CreateFoldersRequest
{
  public required List<BookmarkFolderDto> Folders { get; set; }

}
