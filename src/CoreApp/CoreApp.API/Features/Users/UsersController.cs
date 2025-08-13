using Mediator;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Features.Users;

[Route("users")]
public class UsersController(IMediator mediator)
{
  [HttpPost]
  public async Task<UserResponse> Create(
      [FromBody] Create.Command command,
      CancellationToken cancellationToken
  ) => await mediator.Send(command, cancellationToken);

  [HttpPost("login")]
  public async Task<UserResponse> Login(
      [FromBody] Login.Command command,
      CancellationToken cancellationToken
  ) => await mediator.Send(command, cancellationToken);
}
