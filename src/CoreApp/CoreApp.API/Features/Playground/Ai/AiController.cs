using System.Threading.Tasks;
using CoreApp.API.Domain.Services.ExternalServices;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.API.Features.Playground.Ai
{
    [ApiController]
    [Route("api/playground/ai")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateText([FromBody] string prompt)
        {
            var result = await _aiService.GenerateTextAsync(prompt);
            return Ok(result);
        }
    }
}
