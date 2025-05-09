using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApp.API.Features.Bookmarks
{
  [Route("api/bookmarks")]
  [ApiController]
  public class BookmarksController(IMediator mediator) : ControllerBase
  {
    // POST: api/bookmarks/bulkupload}
    [Route("upload")]
    [HttpPost]
    //[Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task BulkUpload([FromBody] Upload.Upload.Command command, CancellationToken cancellationToken)
    {
      await mediator.Send(command, cancellationToken);
    }


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
