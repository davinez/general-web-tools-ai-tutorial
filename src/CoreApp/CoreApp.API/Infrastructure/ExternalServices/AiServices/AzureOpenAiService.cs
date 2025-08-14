using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Services;
using CoreApp.API.Infrastructure.ExternalServices.ollama.Dto;
using Microsoft.Extensions.Configuration;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
    // For more information on how to use the Azure OpenAI API, see https://learn.microsoft.com/en-us/azure/ai-services/openai/reference
    public class AzureOpenAiService : IAiService
    {
        private readonly OpenAIClient _client;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _options;

        public AzureOpenAiService(IConfiguration configuration)
        {
            _configuration = configuration;
            // The API key for the Azure OpenAI service.
            var apiKey = _configuration["AiService:AzureOpenAI:ApiKey"];
            // The endpoint for the Azure OpenAI service.
            var endpoint = new Uri(_configuration["AiService:AzureOpenAI:Endpoint"]);
            _client = new OpenAIClient(endpoint, new AzureKeyCredential(apiKey));
            _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = false };
        }

        public async Task<BookmarkGroupingResponse> BookmarksGroupingAsync(BookmarkGroupingRequest request, CancellationToken cancellationToken)
        {
            var deploymentName = _configuration["AiService:AzureOpenAI:DeploymentName"];

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = deploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant."),
                    new ChatRequestUserMessage(request.Prompt),
                },
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject
            };

            Response<ChatCompletions> response = await _client.GetChatCompletionsAsync(chatCompletionsOptions, cancellationToken);

            if (response.Value.Choices.Count > 0)
            {
                var responseContent = response.Value.Choices[0].Message.Content;
                var data = JsonSerializer.Deserialize<BookmarkGroupingResponse>(responseContent, _options);
                return data ?? throw new RemoteServiceException(nameof(AzureOpenAiService), $"Error in deserialize response for {responseContent}");
            }

            throw new RemoteServiceException(nameof(AzureOpenAiService), "No response from Azure OpenAI.");
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            // The name of the deployment to use.
            var deploymentName = _configuration["AiService:AzureOpenAI:DeploymentName"];

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = deploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant."),
                    new ChatRequestUserMessage(prompt),
                }
            };

            Response<ChatCompletions> response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            if (response.Value.Choices.Count > 0)
            {
                return response.Value.Choices[0].Message.Content;
            }

            throw new RemoteServiceException(nameof(AzureOpenAiService), "No response from Azure OpenAI.");
        }
    }
}
