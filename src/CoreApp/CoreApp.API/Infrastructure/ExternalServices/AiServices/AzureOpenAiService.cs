using Azure;
using Azure.AI.OpenAI;
using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure.ExternalServices.AiServices.Dto;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.ExternalServices.AiServices
{
  // For more information on how to use the Azure OpenAI API, see https://learn.microsoft.com/en-us/azure/ai-services/openai/reference
  public class AzureOpenAiService : IAiService
  {
    private readonly AzureOpenAIClient _client;
    // The IConfiguration field is no longer needed after the constructor runs.
    // private readonly IConfiguration _configuration; 
    private readonly JsonSerializerOptions _options;
    private readonly string _deploymentName;


    public AzureOpenAiService(IConfiguration configuration)
    {
      // The _configuration field is only used here, so we don't need to store it.
      var apiKey = configuration["AiService:AzureOpenAI:ApiKey"]
          ?? throw new InvalidOperationException("Azure OpenAI API key ('AiService:AzureOpenAI:ApiKey') is not configured.");
      var endpointString = configuration["AiService:AzureOpenAI:Endpoint"]
          ?? throw new InvalidOperationException("Azure OpenAI endpoint ('AiService:AzureOpenAI:Endpoint') is not configured.");

      _deploymentName = configuration["AiService:AzureOpenAI:DeploymentName"]
          ?? throw new InvalidOperationException("Azure OpenAI deployment name ('AiService:AzureOpenAI:DeploymentName') is not configured.");

      var endpoint = new Uri(endpointString);
      var credential = new AzureKeyCredential(apiKey);

      _client = new(endpoint, credential);

      _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };
    }

    public async Task<List<CategorizationResponse>> CategorizeIntoFolderNameAsync(string prompt, CancellationToken cancellationToken)
    {
      try
      {
        ChatClient chatClient = _client.GetChatClient(_deploymentName);

        ChatCompletion completion = await chatClient.CompleteChatAsync(
    [
        // System messages represent instructions or other guidance about how the assistant should behave
        new SystemChatMessage("You are an expert file organizer. Your task is to analyze a list of bookmarks and assign a folder name to each, outputting a valid JSON array."),
        // User messages represent user input, whether historical or the most recent input
        new UserChatMessage(prompt),
    ],
    null,
    cancellationToken);

        // DEBUG LOCALLY AI Response
        // Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

        if (completion.Content.Count > 0)
        {
          string responseContent = completion.Content[0].Text;
          var data = JsonSerializer.Deserialize<List<CategorizationResponse>>(responseContent, _options);
          return data ?? throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(CategorizeIntoFolderNameAsync)}: Failed to deserialize the response from Azure OpenAI.");
        }

        throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(CategorizeIntoFolderNameAsync)}: The response from Azure OpenAI contained no choices.");
      }
      catch (RequestFailedException ex)
      {
        throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(CategorizeIntoFolderNameAsync)}: An error occurred while communicating with Azure OpenAI: {ex.Message}");
      }
    }

    public async Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken)
    {
      try
      {
        ChatClient chatClient = _client.GetChatClient(_deploymentName);

        ChatCompletion completion = await chatClient.CompleteChatAsync(
    [
        // System messages represent instructions or other guidance about how the assistant should behave
        new SystemChatMessage("You are an expert file organizer. Your task is to analyze a list of bookmarks and assign a folder name to each, outputting a valid JSON array."),
        // User messages represent user input, whether historical or the most recent input
        new UserChatMessage(prompt),
    ],
    null,
    cancellationToken);

        Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

        if (completion.Content.Count > 0)
        {
          string responseContent = completion.Content[0].Text;
          var data = JsonSerializer.Deserialize<string>(responseContent, _options);
          return data ?? throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(GenerateTextAsync)}: Failed to deserialize the response from Azure OpenAI.");
        }

        throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(GenerateTextAsync)}: The response from Azure OpenAI contained no choices.");
      }
      catch (RequestFailedException ex)
      {
        throw new RemoteServiceException(nameof(AzureOpenAiService), $"{nameof(GenerateTextAsync)}: An error occurred while communicating with Azure OpenAI: {ex.Message}");
      }
    }
  }
}
