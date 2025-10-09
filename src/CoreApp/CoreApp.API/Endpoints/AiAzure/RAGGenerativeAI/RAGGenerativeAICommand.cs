using Azure;
using Azure.AI.OpenAI;
using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.Security;
using CoreApp.API.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.AiAzure.RAGGenerativeAI;

/*

- When you've created an Azure AI Search index for
your contextual data, you can use it with an OpenAI model. To ground prompts with data from your index, the Azure OpenAI SDK supports extending the request with connection details for the index.

- In this example, the search against the index is keyword-based - in other words,
the query consists of the text in the user prompt, which is matched to text in the indexed documents.
When using an index that supports it, an alternative approach is to use a vector-based query
in which the index and the query use numeric vectors to represent text tokens.
Searching with vectors enables matching based on semantic similarity as well as literal text matches.

To use a vector-based query, you can modify the specification of the Azure AI Search data source
details to include an embedding model; which is then used to vectorize the query text.

rag_params = {
    "data_sources": [
        {
            "type": "azure_search",
            "parameters": {
                "endpoint": search_url,
                "index_name": "index_name",
                "authentication": {
                    "type": "api_key",
                    "key": search_key,
                },
                # Params for vector-based query
                "query_type": "vector",
                "embedding_dependency": {
                    "type": "deployment_name",
                    "deployment_name": "<embedding_model_deployment_name>",
                },
            }
        }
    ],
}


*/


public record RAGGenerativeAICommand : ICommand<RAGGenerativeAIResponse>
{
  public string? Prompt { get; set; }
}

public class RAGGenerativeAICommandHandler
{
  public class CommandValidator : AbstractValidator<RAGGenerativeAICommand>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Prompt).NotNull();
    }
  }

  public class Handler : ICommandHandler<RAGGenerativeAICommand, RAGGenerativeAIResponse>
  {
    private readonly ILogger<RAGGenerativeAICommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly CoreAppContext _context;
    private readonly JsonSerializerOptions _options;
    private readonly IConfiguration _configuration;

    public Handler(
      IConfiguration configuration,
      ILogger<RAGGenerativeAICommandHandler> logger,
      ICurrentUserAccessor currentUserAccessor,
      CoreAppContext context,
      JsonSerializerOptions options)
    {
      _configuration = configuration;
      _logger = logger;
      _currentUserAccessor = currentUserAccessor;
      _context = context;
      _options = options;
    }


    // PENDING use index and rag flow
    // https://microsoftlearning.github.io/mslearn-ai-studio/Instructions/04-Use-own-data.html
    public async ValueTask<RAGGenerativeAIResponse> Handle(
        RAGGenerativeAICommand command,
        CancellationToken cancellationToken
    )
    {
      try
      {
        // Get an Azure OpenAI chat client

        var apiKey = _configuration["AiService:AzureOpenAI:ApiKey"]
            ?? throw new InvalidOperationException("Azure OpenAI API key ('AiService:AzureOpenAI:ApiKey') is not configured.");
        var endpointString = _configuration["AiService:AzureOpenAI:Endpoint"]
            ?? throw new InvalidOperationException("Azure OpenAI endpoint ('AiService:AzureOpenAI:Endpoint') is not configured.");

        var deploymentName = _configuration["AiService:AzureOpenAI:DeploymentName"]
            ?? throw new InvalidOperationException("Azure OpenAI deployment name ('AiService:AzureOpenAI:DeploymentName') is not configured.");

        var endpoint = new Uri(endpointString);
        var credential = new AzureKeyCredential(apiKey);

        // Can i specify api version like python sdk? api_version = "2024-12-01-preview",
        var openAiClient = new AzureOpenAIClient(
          endpoint,
          credential
          );


        // Initialize prompt with system message
        var chatMessages = new ChatMessage[]
        {
            // System messages represent instructions or other guidance about how the assistant should behave
            new SystemChatMessage("You are an expert file organizer. Your task is to analyze a list of bookmarks and assign a folder name to each, outputting a valid JSON array."),
            // User messages represent user input, whether historical or the most recent input
            new UserChatMessage(command.Prompt ?? throw new CoreAppException("Null prompt in RAGGenerativeAICommand")),
        };


        ChatClient chatClient = openAiClient.GetChatClient(deploymentName);

        // Submit the prompt with the index information
        ChatCompletion response = await chatClient.CompleteChatAsync(
          messages: chatMessages,
          null,
          cancellationToken);

        // Print the contextualized response
        if (response.Content.Count > 0)
        {
          string responseContent = response.Content[0].Text;

          return new RAGGenerativeAIResponse
          {
            Response = responseContent
          };
        }

      }
      catch (Exception ex)
      {
        _logger.LogError("Error in AI Client process");

        throw new CoreAppException("Error in AI Client process", ex);
      }


      return new RAGGenerativeAIResponse();
    }


    private string GetModelName()
    {
      string? modelName = _configuration["Ai102:AzureAIFoundry:ModelName"];

      if (string.IsNullOrWhiteSpace(modelName))
      {
        throw new CoreAppException("Empty Ai102:AzureAIFoundry:ModelName config");
      }

      return modelName;
    }

  }
}

