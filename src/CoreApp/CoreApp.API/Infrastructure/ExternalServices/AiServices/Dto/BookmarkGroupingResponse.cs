using System;
using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto
{
  public class BookmarkGroupingResponse
  {
    public List<BookmarkFolderDto> WithFolder { get; set; } = [];
  }

}
