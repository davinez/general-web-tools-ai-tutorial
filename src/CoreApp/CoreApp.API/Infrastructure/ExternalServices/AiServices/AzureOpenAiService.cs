using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
    public class AzureOpenAiService : IAiService
    {
        private readonly OpenAIClient _client;
        private readonly IConfiguration _configuration;

        public AzureOpenAiService(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = _configuration["AiService:AzureOpenAI:ApiKey"];
            var endpoint = new Uri(_configuration["AiService:AzureOpenAI:Endpoint"]);
            _client = new OpenAIClient(endpoint, new AzureKeyCredential(apiKey));
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
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
