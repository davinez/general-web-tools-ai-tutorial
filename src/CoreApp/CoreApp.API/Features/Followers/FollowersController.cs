using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Features.Profiles;
using CoreApp.API.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.API.Features.Followers;

[Route("profiles")]
public class FollowersController(IMediator mediator) : Controller
{
    [HttpPost("{username}/follow")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task<ProfileEnvelope> Follow(string username, CancellationToken cancellationToken) =>
        mediator.Send(new Add.Command(username), cancellationToken);

    [HttpDelete("{username}/follow")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task<ProfileEnvelope> Unfollow(string username, CancellationToken cancellationToken) =>
        mediator.Send(new Delete.Command(username), cancellationToken);
}
