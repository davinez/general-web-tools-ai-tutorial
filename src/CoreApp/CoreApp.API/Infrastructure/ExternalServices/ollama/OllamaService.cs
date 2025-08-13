using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.ollama;

public class OllamaService : IOllamaService
{

  private readonly HttpClient _client;
  private readonly JsonSerializerOptions _options;

  public OllamaService(IConfiguration configuration, HttpClient client)
  {
    _client = client;
    _client.BaseAddress = new Uri(configuration["OllamaService:BaseAddress"] ?? throw new CoreAppException($"Empty config section in {nameof(OllamaService)} BaseAdress"));
    // _client.Timeout = new TimeSpan(0, 0, 30);

    _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };
  }

  public async Task<ProcessBookmarkGroupingResponse> ProcessBookmarksGroupingAsync(
        ProcessBookmarkGroupingRequest request,
        CancellationToken cancellationToken
    )
  {

    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = false,
      Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    string requestJson = System.Text.Json.JsonSerializer.Serialize(request, options);

    using StringContent jsonContent = new(
    requestJson,
    Encoding.UTF8,
    "application/json"
    );

    using var response = await _client.PostAsync("api/generate", jsonContent, cancellationToken);

    if (!response.IsSuccessStatusCode)
      throw new RemoteServiceException(nameof(OllamaService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Request: {requestJson}");

    var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

    using var doc = JsonDocument.Parse(responseJson);
    var dataElement = doc.RootElement.GetProperty("response");
    var responseNode = dataElement.GetString() ?? throw new RemoteServiceException(nameof(OllamaService), $"Error in deserialize response for {responseJson}");

    var data = JsonSerializer.Deserialize<ProcessBookmarkGroupingResponse>(responseNode, _options) ?? throw new RemoteServiceException(nameof(OllamaService), $"Error in deserialize response node for {responseJson}");

    return data ?? throw new RemoteServiceException(nameof(OllamaService), $"Null Data Api Response for request {requestJson}");
  }

}
