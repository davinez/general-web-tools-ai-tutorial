using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
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

        public async Task<string> GenerateTextAsync(string prompt)
        {
            var apiKey = _configuration["AiService:Gemini:ApiKey"];
            var modelName = _configuration["AiService:Gemini:ModelName"];

            var requestUri = $"/v1beta/models/{modelName}:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
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
}
