using Azure.AI.Projects;
using Azure.Identity;
using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.Security;
using CoreApp.API.Endpoints.AiAzure.GenerativeAIChat;
using CoreApp.API.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.AiAzure.AIFoundrySDK;


/*

1- Step 1: Set up Azure AI Foundry
The Azure AI Foundry SDK (azure.ai.projects) provides a set of endpoints to interact
with Azure AI Foundry resources and projects.
The SDK includes the following endpoints (availables ath the overview tab):

- An endpoint for the project itself; which can be used to access project connections, agents, and models in the Azure AI Foundry resource.
- An endpoint for Azure OpenAI Service APIs in the project's Azure AI Foundry resource.
- An endpoint for Azure AI services APIs (such as Azure AI Vision and Azure AI Language) in the Azure AI Foundry resource.

2- Authentication and Authorization

DefaultAzureCredential tries the following credential types in order:

--- Automated/Production Environments ---
1. EnvironmentCredential: Reads credentials from environment variables.
2. WorkloadIdentityCredential: Authenticates in Kubernetes and other webhook-enabled environments.
3. ManagedIdentityCredential: Uses the managed identity of the Azure host (VM, App Service, etc.).

--- Local Development Environments ---
4. SharedTokenCacheCredential: Uses tokens from the shared cache populated by Microsoft apps.
5. VisualStudioCredential: Uses the account signed into Visual Studio.
6. VisualStudioCodeCredential: Uses the account signed into the VS Code Azure extension.
7. AzureCliCredential: Uses credentials from the Azure CLI ('az login').
8. AzurePowerShellCredential: Uses credentials from Azure PowerShell ('Connect-AzAccount').
9. AzureDeveloperCliCredential: Uses credentials from the Azure Developer CLI ('azd auth login').

--- Interactive ---
10. InteractiveBrowserCredential: Prompts the user for login via the system's browser.

*/






public record GenerativeAIChatCommand() : ICommand<GenerativeAIChatResponse>
{
  public string? Prompt { get; set; }
}

public class GenerativeAIChatCommandHandler
{
  public class CommandValidator : AbstractValidator<GenerativeAIChatCommand>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Prompt).NotNull();
    }
  }

  public class Handler : ICommandHandler<GenerativeAIChatCommand, GenerativeAIChatResponse>
  {
    private readonly ILogger<GenerativeAIChatCommandHandler> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly CoreAppContext _context;
    private readonly JsonSerializerOptions _options;
    private readonly IConfiguration _configuration;

    public Handler(
      IConfiguration configuration,
      ILogger<GenerativeAIChatCommandHandler> logger,
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

    public async ValueTask<GenerativeAIChatResponse> Handle(
        GenerativeAIChatCommand command,
        CancellationToken cancellationToken
    )
    {
      Uri projectEndpoint = GetAzureFoundryProjectEndpoint();
      var credentialAzure = new DefaultAzureCredential();

      var projectClient = new AIProjectClient(projectEndpoint, credentialAzure);

      ListConnectedResources(projectClient);

      /*
      # Chat client

      A common scenario in an AI application is to connect to a generative AI model and use prompts to engage
      in a chat-based dialog with it.

      While you can use the Azure OpenAI SDK, to connect "directly" to a model using key-based
      or Microsoft Entra ID authentication; when your model is deployed in an Azure AI Foundry project,
      you can also use the Azure AI Foundry SDK to retrieve a project client, from which you can then get an
      authenticated OpenAI chat client for any models deployed in the project's Azure AI Foundry resource.

      This approach makes it easy to write code that consumes models deployed in your project, switching between
      them easily by changing the model deployment name parameter.

      Tip
      You can use the OpenAI chat client provided by an Azure AI Foundry project to chat with any model
      */

      try
      {
        // Get the chat client
        OpenAIClient openAIClient = projectClient.GetOpenAIClient(connectionName: "ssss", apiVersion: "2024-10-21");
        string modelName = GetModelName();
        string userPrompt = command.Prompt ?? throw new CoreAppException("Null prompt in GenerativeAIChatCommand");

        ChatCompletion response = await openAIClient.GetChatClient(modelName).CompleteChatAsync(
                       [
                           // System messages represent instructions or other guidance about how the assistant should behave
                           new SystemChatMessage("You are an azure tech specialist."),
                           // User messages represent user input, whether historical or the most recent input
                           new UserChatMessage(userPrompt),
                       ],
                       null,
                       cancellationToken);

        if (response.Content.Count > 0)
        {
          string responseContent = response.Content[0].Text;

          return new GenerativeAIChatResponse
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





      return new GenerativeAIChatResponse();
    }

    private void ListConnectedResources(AIProjectClient projectClient)
    {
      /*
            # Connected Resources

            These are defined both at the parent (Azure AI Foundry resource or hub) level,
            and at the project level. Each resource is a connection to an external service,
            such as Azure storage, Azure AI Search, Azure OpenAI, or another Azure AI Foundry resource.      
      */
      var connectedResources = projectClient.Connections.GetConnections();

      foreach (var resource in connectedResources)
      {
        string details = $"Connection Name: {resource.Name}, Type: {resource.Type}";
      }
    }

    private Uri GetAzureFoundryProjectEndpoint()
    {
      string? projectEndpoint = _configuration["Ai102:AzureAIFoundry:ProjectEndpoint"];

      if (string.IsNullOrWhiteSpace(projectEndpoint))
      {
        throw new CoreAppException("Empty Ai102:AzureAIFoundry:ProjectEndpoint config");
      }

      try
      {
        var myUri = new Uri(projectEndpoint);
        // You can now access parts of the URI
        //string host = myUri.Host; // "www.example.com"
        //string scheme = myUri.Scheme; // "https"
        //string pathAndQuery = myUri.PathAndQuery; // "/path?query=value"
        return myUri;
      }
      catch (Exception ex)
      {
        _logger.LogError("Error extracting URI Format for Azure AI Foundry Project Endpoint");

        throw new CoreAppException("Error extracting URI Format for Azure AI Foundry Project Endpoint", ex);
      }
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
