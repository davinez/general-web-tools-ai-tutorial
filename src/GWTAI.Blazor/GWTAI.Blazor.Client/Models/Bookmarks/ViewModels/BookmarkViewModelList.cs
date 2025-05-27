using GWTAI.Blazor.Client.Models.Bookmarks.Dtos;

namespace GWTAI.Blazor.Client.Models.Bookmarks.ViewModels;

public class BookmarkViewModelList
{
  public IEnumerable<BookmarkDto>? Bookmarks { get; set; }

  public int BookmarksCount { get; set; }
}
