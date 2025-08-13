using CoreApp.API.Features.Bookmarks.CreateFolders;
using CoreApp.API.Features.Bookmarks.Upload;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApp.API.Features.Bookmarks
{

  [Route("api/bookmarks")]
  [ApiController]
  public class BookmarksController(IMediator mediator) : ControllerBase
  {


    // POST: api/bookmarks/bulk-upload
    [Route("bulk-upload")]
    [HttpPost]
    // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> BulkUpload(IFormFile fileContent, [FromForm] string fileName, [FromForm] DateTimeOffset uploadTimestamp)
    {

      // Now you can pass this data to your mediator or service
      var request = new UploadRequest
      {
        FileContent = fileContent,
        FileName = fileName,
        UploadTimestamp = uploadTimestamp
      };

      await mediator.Send(new UploadCommand(request));

      return Ok();
    }

    // POST: api/bookmarks/create-folders
    [Route("create-folders")]
    [HttpPost]
    // [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> CreateFolders([FromBody] CreateFoldersRequest request)
    {

      await mediator.Send(new CreateFoldersCommand(request));

      return Ok();
    }

    /* Reference Endpoints */

    // GET api/<BookmarksController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<BookmarksController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<BookmarksController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<BookmarksController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
