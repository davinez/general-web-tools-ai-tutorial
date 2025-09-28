using CoreApp.API.Domain.Models;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Endpoints.AiAzure.AIFoundrySDK;
using CoreApp.API.Endpoints.AiAzure.GenerativeAIChat;
using CoreApp.API.Endpoints.Bookmarks.Upload;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.Ai
{
  [ApiController]
  [Route("api/ai-azure")]
  public class AiAzureController : ControllerBase
  {
    private readonly IAiService _aiService;
    private readonly CancellationToken _cancellationToken;
    private readonly IMediator _mediator;


    public AiAzureController(IAiService aiService, IMediator mediator, CancellationToken cancellationToken)
    {
      _aiService = aiService;
      _mediator = mediator;
      _cancellationToken = cancellationToken;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateText([FromBody] string prompt)
    {
      var result = await _aiService.GenerateTextAsync(prompt, _cancellationToken);
      return Ok(result);
    }

    [Route("generative-chat-azure-foundry-sdk")]
    [HttpPost]
    // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> GenerativeChatFoundrySDK(GenerativeAIChatCommand command)
    {
      GenerativeAIChatResponse response = await _mediator.Send(command);

      var data = new ApiResponse<GenerativeAIChatResponse> { Data = response };
      return Ok(data);
    }

  }
}
