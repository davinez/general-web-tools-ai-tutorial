using GWTAI.Blazor.Client.Models.Bookmarks.Dtos;
using GWTAI.Blazor.Client.Models.Bookmarks.Requests;
using GWTAI.Blazor.Client.Shared.Data;

namespace GWTAI.Blazor.Client.Services.Contracts
{
  public interface IBookmarkService
  {
    Task<PagedResult<BookmarkDto>> GetBookmarks(string name, string page);

    Task<BookmarkDto> GetBookmark(int id);

    Task DeleteBookmark(int id);

    Task UploadBookmarks(BulkUploadDto bulkUpload);

  }
}
