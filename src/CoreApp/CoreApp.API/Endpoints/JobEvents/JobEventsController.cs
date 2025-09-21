using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.API.Domain.Security;
using CoreApp.API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Endpoints.JobEvents;

// DTOs for the response, matching the frontend model expectations.
public class JobEventDto
{
    public Guid UploadId { get; set; }
    public string UserId { get; set; }
    public DateTime EventTimestamp { get; set; }
    public string Status { get; set; }
    public string Content { get; set; }
}

public class JobEventWithWorkflowDto : JobEventDto
{
    public string Workflow { get; set; }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
                    UploadId = je.JobEventId,
                    UserId = je.UserId,
                    EventTimestamp = je.EventTimestamp,
                    Status = je.Status,
                    Content = je.Content
                })
                .ToListAsync();
            return Ok(results);
        }
        else
        {
            var results = await queryable
                .OrderByDescending(je => je.EventTimestamp)
                .Select(je => new JobEventWithWorkflowDto
                {
                    UploadId = je.JobEventId,
                    UserId = je.UserId,
                    EventTimestamp = je.EventTimestamp,
                    Status = je.Status,
                    Content = je.Content,
                    Workflow = je.Workflow
                })
                .ToListAsync();
            return Ok(results);
        }
    }

    [HttpGet("{eventId}")]
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
