using System.Net.Http.Json;
using GWTAI.Blazor.Client.Models.Bookmarks.Dtos;
using GWTAI.Blazor.Client.Models.Bookmarks.Requests;
using GWTAI.Blazor.Client.Services.Contracts;
using GWTAI.Blazor.Client.Shared.Data;

namespace GWTAI.Blazor.Client.Services;

public class BookmarkService : IBookmarkService
{


  private readonly HttpClient _httpClient;

  public BookmarkService(IHttpClientFactory httpClientFactory)
  {
    _httpClient = httpClientFactory.CreateClient("CoreApi");
  }



  public async Task<PagedResult<BookmarkDto>> GetBookmarks(string name, string page)
  {
    var response = await _httpClient.GetFromJsonAsync<PagedResult<BookmarkDto>>($"api/bookmarks?name={name}&page={page}");
    return response ?? new PagedResult<BookmarkDto>();
  }

  public async Task<BookmarkDto> GetBookmark(int id)
  {
    var response = await _httpClient.GetFromJsonAsync<BookmarkDto>($"api/bookmarks/{id}");
    return response ?? new BookmarkDto();
  }

  public async Task DeleteBookmark(int id)
  {
    await _httpClient.DeleteAsync($"api/bookmarks/{id}");
  }

  public async Task UploadBookmarks(BulkUploadDto bulkUpload)
  {

    var request = new BulkUploadBookmarksRequest()
    {
      FileContent = bulkUpload.FileContent,
      FileName = bulkUpload.FileName,
      UploadTimestamp = DateTime.Now,
    };

    // Using multipart form-data for large file uploads
    using var content = new MultipartFormDataContent();

    // Add file content
    var fileContent = new ByteArrayContent(request.FileContent);
    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
    content.Add(fileContent, "FileContent", request.FileName);

    // Add additional properties as form fields
    content.Add(new StringContent(request.FileName), "FileName");
    content.Add(new StringContent(request.UploadTimestamp.ToString("o")), "UploadTimestamp"); // ISO 8601 format

    var response = await _httpClient.PostAsync("api/bookmarks/bulk-upload", content);

    if (!response.IsSuccessStatusCode)
    {
      var error = await response.Content.ReadAsStringAsync();
      throw new Exception($"Failed to upload: {error}");
    }

  }

}
