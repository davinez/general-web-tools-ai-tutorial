using CoreApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.JobEvents;

[ApiController]
[Route("api/[controller]")]
public class JobEventsController(CoreAppContext context) : ControllerBase
{

  // ... your DbContext or repository

  [HttpGet("{eventId}")]
  public async Task<IActionResult> GetStatus(string eventId)
  {
    // Retrieve the event from the database
    var jobEvent = await context.JobEvents.FindAsync(eventId);

    if (jobEvent == null)
    {
      return NotFound();
    }

    return Ok(new { jobEventId = jobEvent.JobEventId, status = jobEvent.Status });
  }
}
