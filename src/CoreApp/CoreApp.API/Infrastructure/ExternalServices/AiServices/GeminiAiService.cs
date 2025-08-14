using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Services;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using Microsoft.Extensions.Configuration;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
    // For more information on how to use the Gemini API, see https://ai.google.dev/docs/gemini_api_overview
    public class GeminiAiService : IAiService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _options;

        public GeminiAiService(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
            _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<BookmarkGroupingResponse> BookmarksGroupingAsync(BookmarkGroupingRequest request, CancellationToken cancellationToken)
        {
            var apiKey = _configuration["AiService:Gemini:ApiKey"];
            var modelName = _configuration["AiService:Gemini:ModelName"];

            var requestUri = $"/v1beta/models/{modelName}:generateContent?key={apiKey}";

            var requestBody = new GeminiRequest
            {
                Contents = new[]
                {
                    new Content
                    {
                        Parts = new[]
                        {
                            new Part { Text = request.Prompt }
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

            using var response = await _client.PostAsync(requestUri, jsonContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new RemoteServiceException(nameof(GeminiAiService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Content: {errorContent} Request: {requestJson}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            using var doc = JsonDocument.Parse(responseJson);
            var dataElement = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text");
            var responseText = dataElement.GetString();

            var data = JsonSerializer.Deserialize<BookmarkGroupingResponse>(responseText, _options);

            return data ?? throw new RemoteServiceException(nameof(GeminiAiService), $"Error in deserialize response for {responseJson}");
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            // The API key for the Gemini service.
            var apiKey = _configuration["AiService:Gemini:ApiKey"];
            // The name of the model to use.
            var modelName = _configuration["AiService:Gemini:ModelName"];

            var requestUri = $"/v1beta/models/{modelName}:generateContent?key={apiKey}";

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

            using var response = await _client.PostAsync(requestUri, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new RemoteServiceException(nameof(GeminiAiService), $"Error: {response.StatusCode} Message: {response.ReasonPhrase} Content: {errorContent} Request: {requestJson}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var dataElement = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text");
            var responseText = dataElement.GetString();

            return responseText ?? throw new RemoteServiceException(nameof(GeminiAiService), $"Error in deserialize response for {responseJson}");
        }
    }

    // The request body for the Gemini API.
    public class GeminiRequest
    {
        // The contents of the request.
        public Content[] Contents { get; set; }
    }

    // The content of the request.
    public class Content
    {
        // The parts of the content.
        public Part[] Parts { get; set; }
    }

    // The part of the content.
    public class Part
    {
        // The text of the part.
        public string Text { get; set; }
    }
}
