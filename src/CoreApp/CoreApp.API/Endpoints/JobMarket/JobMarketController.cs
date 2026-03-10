using CoreApp.API.Domain.Models;
using CoreApp.API.Endpoints.JobMarket.FetchJobs;
using CoreApp.API.Endpoints.JobMarket.Upload;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.JobMarket;

[Route("api/jobmarket")]
[ApiController]
public class JobMarketController(IMediator mediator) : ControllerBase
{
  /// <summary>
  /// Accepts 1-N screenshot files (PNG/JPEG/WebP) and lands them in the
  /// Azure Blob Storage Bronze layer under bronze/screenshots/{date}/.
  /// Returns a JobEventId that can be used to track processing status via SignalR.
  /// </summary>
  [HttpPost("screenshots/upload")]
  [Consumes("multipart/form-data")]
  // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
  public async Task<IActionResult> UploadScreenshots(
      [FromForm] IFormFileCollection files,
      [FromForm] string uploadTimestamp)
  {
    var request = new UploadJobScreenshotsRequest
    {
      Files = files,
      UploadTimestamp = uploadTimestamp,
    };

    var response = await mediator.Send(new UploadJobScreenshotsCommand(request));
    return Ok(new ApiResponse<UploadJobScreenshotsResponse> { Data = response });
  }

  /// <summary>
  /// Triggers the IJobProvider registry to fetch job postings.
  /// Each provider is guarded by a daily-lock (Redis): if already called today,
  /// that provider is skipped and the cached result is reported.
  /// Raw results are uploaded to the Bronze Blob layer as JSON.
  /// </summary>
  [HttpPost("fetch")]
  // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
  public async Task<IActionResult> FetchJobs([FromBody] FetchJobsRequest request)
  {
    var response = await mediator.Send(new FetchJobsCommand(request));
    return Ok(new ApiResponse<FetchJobsResponse> { Data = response });
  }
}
