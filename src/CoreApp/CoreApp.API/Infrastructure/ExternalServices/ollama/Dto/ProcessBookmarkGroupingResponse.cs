using System;
using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama.Dto
{
  public class ProcessBookmarkGroupingResponse
  {
    //public ResponseAI Response { get; set; } = new ResponseAI();
    public List<BookmarkFolderDto> WithFolder { get; set; } = new List<BookmarkFolderDto>();
    public List<BookmarkDto> WithoutFolder { get; set; } = new List<BookmarkDto>();
  }

  public class ResponseAI
  {
    public List<BookmarkFolderDto> WithFolder { get; set; } = new List<BookmarkFolderDto>();
    public List<BookmarkDto> WithoutFolder { get; set; } = new List<BookmarkDto>();
  }

}
