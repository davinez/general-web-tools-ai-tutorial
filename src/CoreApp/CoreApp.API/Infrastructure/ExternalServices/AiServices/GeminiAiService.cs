using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
  // For more information on how to use the Gemini API, see https://ai.google.dev/docs/gemini_api_overview
  public class GeminiAiService : IAiService
  {
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _options;

    public GeminiAiService(IConfiguration configuration, HttpClient client, JsonSerializerOptions options)
    {
      _configuration = configuration;
      _client = client;
      // _client.Timeout = new TimeSpan(0, 0, 30);
      _options = options;
    }

    public async Task<List<CategorizationResponse>> CategorizeIntoFolderNameAsync(string prompt, CancellationToken cancellationToken)
    {
      var apiKey = _configuration["AiService:Gemini:ApiKey"];
      var modelName = _configuration["AiService:Gemini:ModelName"];
      var requestUri = $"v1beta/models/{modelName}:generateContent";

      var requestBody = new GeminiRequest
      {
        Contents =
          [
              new Content
                    {
                        Parts =
                        [
                            new Part { Text = prompt }
                        ]
                    }
          ]
      };

      string requestJson = JsonSerializer.Serialize(requestBody, _options);

      using StringContent jsonContent = new(
          requestJson,
          Encoding.UTF8,
          "application/json"
      );

      using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
      request.Headers.Add("x-goog-api-key", apiKey);
      request.Content = jsonContent;

      using var response = await _client.SendAsync(request, cancellationToken);

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new RemoteServiceException(nameof(GeminiAiService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Content: {errorContent} Request: {requestJson}");
      }

      var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

      using var doc = JsonDocument.Parse(responseJson);
      JsonElement dataElement = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text");
      string responseText = dataElement.GetString() ?? throw new RemoteServiceException(nameof(GeminiAiService), $"Error in deserialize response");

      List<CategorizationResponse> data = JsonSerializer.Deserialize<List<CategorizationResponse>>(responseText, _options) ?? throw new RemoteServiceException(nameof(GeminiAiService), $"Error in deserialize response node");

      return data;
    }

    public async Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken)
    {
      // The API key for the Gemini service.
      var apiKey = _configuration["AiService:Gemini:ApiKey"];
      // The name of the model to use.
      var modelName = _configuration["AiService:Gemini:ModelName"];

      var requestUri = $"v1beta/models/{modelName}:generateContent";

      var requestBody = new GeminiRequest
      {
        Contents = new[]
          {
                    new Content
                    {
                        Parts = new[]
                        {
                            new Part { Text = prompt }
                        }
                    }
                }
      };

      string requestJson = JsonSerializer.Serialize(requestBody, _options);

      using StringContent jsonContent = new(
          requestJson,
          Encoding.UTF8,
          "application/json"
      );

      using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
      request.Headers.Add("x-goog-api-key", apiKey);
      request.Content = jsonContent;

      using var response = await _client.SendAsync(request, cancellationToken);

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new RemoteServiceException(nameof(GeminiAiService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Content: {errorContent} Request: {requestJson}");
      }

      var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

      using var doc = JsonDocument.Parse(responseJson);
      var dataElement = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text");
      string? responseText = dataElement.GetString();

      return responseText ?? throw new RemoteServiceException(nameof(GeminiAiService), $"Error in deserialize response for {responseJson}");
    }
  }

  // The request body for the Gemini API.
  public class GeminiRequest
  {
    // The contents of the request.
    public required Content[] Contents { get; set; }
  }

  // The content of the request.
  public class Content
  {
    // The parts of the content.
    public required Part[] Parts { get; set; }
  }

  // The part of the content.
  public class Part
  {
    // The text of the part.
    public required string Text { get; set; }
  }
}
