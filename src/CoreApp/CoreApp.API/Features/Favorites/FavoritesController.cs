using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Features.Articles;
using CoreApp.API.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.API.Features.Favorites;

[Route("articles")]
public class FavoritesController(IMediator mediator) : Controller
{
    [HttpPost("{slug}/favorite")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task<ArticleEnvelope> FavoriteAdd(string slug, CancellationToken cancellationToken) =>
        mediator.Send(new Add.Command(slug), cancellationToken);

    [HttpDelete("{slug}/favorite")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task<ArticleEnvelope> FavoriteDelete(string slug, CancellationToken cancellationToken) =>
        mediator.Send(new Delete.Command(slug), cancellationToken);
}
