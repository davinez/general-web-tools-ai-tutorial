using System;
using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama.Dto
{
  public class ProcessBookmarkGroupingResponse
  {
    public List<BookmarkFolderDto> WithFolder { get; set; } = new List<BookmarkFolderDto>();
    public List<BookmarkDto> WithoutFolder { get; set; } = new List<BookmarkDto>();
  }

}
