using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Security;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Features.Users;

[Route("user")]
[Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
public class UserController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
{
  [HttpGet]
  public async ValueTask<UserResponse> GetCurrent(CancellationToken cancellationToken)
  {
    return await mediator.Send(
          new Details.Query(currentUserAccessor.GetCurrentUsername() ?? "<unknown>"),
          cancellationToken
      );
  }

  [HttpPut]
  public async Task<UserResponse> UpdateUser(
      [FromBody] Edit.Command command,
      CancellationToken cancellationToken
  )
  {
    return await mediator.Send(command, cancellationToken);
  }
}
