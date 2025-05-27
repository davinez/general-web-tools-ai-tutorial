using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Comments;

public class List
{
    public record Query(string Slug) : IRequest<CommentsEnvelope>;

    public class QueryHandler(CoreAppContext context) : IRequestHandler<Query, CommentsEnvelope>
    {
        public async Task<CommentsEnvelope> Handle(
            Query message,
            CancellationToken cancellationToken
        )
        {
            var article = await context
                .Articles.Include(x => x.Comments)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

            if (article == null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }

            return new CommentsEnvelope(article.Comments);
        }
    }
}
