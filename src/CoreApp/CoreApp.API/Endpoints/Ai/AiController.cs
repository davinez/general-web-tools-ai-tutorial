using CoreApp.API.Domain.Services.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.Ai
{
    [ApiController]
    [Route("api/playground/ai")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly CancellationToken _cancellationToken;

    public AiController(IAiService aiService, CancellationToken cancellationToken)
        {
            _aiService = aiService;
            _cancellationToken = cancellationToken;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateText([FromBody] string prompt)
        {
            var result = await _aiService.GenerateTextAsync(prompt, _cancellationToken);
            return Ok(result);
        }
    }
}
