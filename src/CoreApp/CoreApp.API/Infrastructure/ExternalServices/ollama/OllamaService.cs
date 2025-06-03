using CoreApp.API.Features.Bookmarks.Upload;
using System.Threading.Tasks;
using System.Threading;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using CoreApp.API.Infrastructure.Errors;
using System.Text;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama;

public class OllamaService: IOllamaService
{

  private readonly HttpClient _client;
  private readonly JsonSerializerOptions _options;

  public OllamaService(IConfiguration configuration, HttpClient client)
  {
    _client = client;
    _client.BaseAddress = new Uri(configuration["OllamaService:BaseAddress"] ?? throw new CoreAppException($"Empty config section in {nameof(CoreAppException)} BaseAdress"));
    // _client.Timeout = new TimeSpan(0, 0, 30);

    _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };
  }

  public async Task<ProcessBookmarkGroupingRequest> ProcessBookmarksGrouping(
        ProcessBookmarkGroupingRequest request,
        CancellationToken cancellationToken
    )
  {
    var requestJson = JsonSerializer.Serialize(request);

    using StringContent jsonContent = new(
    requestJson,
    Encoding.UTF8,
    "application/json"
    );

    using var response = await _client.PostAsync("/api/generate", jsonContent, cancellationToken);

    if (!response.IsSuccessStatusCode)
      throw new RemoteServiceException(nameof(OllamaService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Request: {requestJson}");

    var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

    var data = JsonSerializer.Deserialize<ProcessBookmarkGroupingRequest>(responseJson, _options) ?? throw new RemoteServiceException(nameof(OllamaService), $"Error in deserialize response for {responseJson}");

    return data ?? throw new RemoteServiceException(nameof(OllamaService), $"Null Data Api Response for request {requestJson}");
  }

}
