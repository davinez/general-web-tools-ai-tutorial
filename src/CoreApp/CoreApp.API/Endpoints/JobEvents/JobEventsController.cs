using CoreApp.API.Domain.Models;
using CoreApp.API.Domain.Security;
using CoreApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.JobEvents;


public record JobEventDto
{
  public Guid JobEventId { get; set; }
  public required string UserId { get; set; }
  public DateTime EventTimestamp { get; set; }
  public required string Status { get; set; }
  public required string Content { get; set; }
  public string? Workflow { get; set; }
}

[ApiController]
[Route("api/job-events")]
//[Authorize]
public class JobEventsController : ControllerBase
{
  private readonly CoreAppContext _context;
  private readonly ICurrentUserAccessor _currentUserAccessor;

  public JobEventsController(CoreAppContext context, ICurrentUserAccessor currentUserAccessor)
  {
    _context = context;
    _currentUserAccessor = currentUserAccessor;
  }

  [HttpGet]
  public async Task<IActionResult> GetJobEvents([FromQuery] string? workflow)
  {
    var userId = _currentUserAccessor.GetCurrentUsername();
    var queryable = _context.JobEvents
        .AsNoTracking()
        .Where(je => je.UserId == userId);

    if (!string.IsNullOrEmpty(workflow))
    {
      queryable = queryable.Where(je => je.Workflow == workflow);

      var results = await queryable
          .OrderByDescending(je => je.EventTimestamp)
          .Select(je => new JobEventDto
          {
            JobEventId = je.JobEventId,
            UserId = je.UserId,
            EventTimestamp = je.EventTimestamp,
            Status = je.Status,
            Content = je.Content
          })
          .ToListAsync();

      var data = new ApiResponse<IEnumerable<JobEventDto>> { Data = results };
      return Ok(data);
    }
    else
    {
      var results = await queryable
          .OrderByDescending(je => je.EventTimestamp)
          .Select(je => new JobEventDto
          {
            JobEventId = je.JobEventId,
            UserId = je.UserId,
            EventTimestamp = je.EventTimestamp,
            Status = je.Status,
            Content = je.Content,
            Workflow = je.Workflow
          })
          .ToListAsync();

      var data = new ApiResponse<IEnumerable<JobEventDto>> { Data = results };
      return Ok(data);
    }
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetJobEventsById(string id, [FromQuery] string workflow)
  {
    var userId = _currentUserAccessor.GetCurrentUsername();
    var result = await _context.JobEvents
        .AsNoTracking()
        .Where(je => je.UserId == userId
                    && je.Workflow == workflow
                    && je.JobEventId == Guid.Parse(id))
        .OrderByDescending(je => je.EventTimestamp)
        .Select(je => new JobEventDto
        {
          JobEventId = je.JobEventId,
          UserId = je.UserId,
          EventTimestamp = je.EventTimestamp,
          Status = je.Status,
          Content = je.Content
        })
        .SingleOrDefaultAsync();

    var data = new ApiResponse<JobEventDto> { Data = result };
    return Ok(data);
  }


  // Not in use, but could be useful for checking status by event ID.
  [HttpGet]
  [Route("status/{eventId}")]
  public async Task<IActionResult> GetStatus(string eventId)
  {
    // Retrieve the event from the database, ensuring it belongs to the current user.
    var userId = _currentUserAccessor.GetCurrentUsername();
    var jobEvent = await _context.JobEvents.FirstOrDefaultAsync(je => je.JobEventId.ToString() == eventId && je.UserId == userId);

    if (jobEvent == null)
    {
      return NotFound();
    }

    return Ok(new { jobEventId = jobEvent.JobEventId, status = jobEvent.Status });
  }
}
