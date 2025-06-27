using System;
using System.Collections.Generic;
using CoreApp.API.Features.Bookmarks.Dtos;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama.Dto
{
  public class ProcessBookmarkGroupingResponse
  { 
    public ResponseAI Response { get; set; } = new ResponseAI();
  }

  public class ResponseAI
  {
    public List<BookmarkFolderDto> Folder { get; set; } = new List<BookmarkFolderDto>();
    public List<BookmarkDto> Bookmarks { get; set; } = new List<BookmarkDto>();

    public SuggestionsAI? SuggestionsAI { get; set; }
  }


  public class SuggestionsAI
  {
    public string[] PossibleDuplicate { get; set; } = Array.Empty<string>();
  }
}
