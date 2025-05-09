using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Articles;

public class Details
{
    public record Query(string Slug) : IRequest<ArticleEnvelope>;

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(CoreAppContext context) : IRequestHandler<Query, ArticleEnvelope>
    {
        public async Task<ArticleEnvelope> Handle(
            Query message,
            CancellationToken cancellationToken
        )
        {
            var article = await context
                .Articles.GetAllData()
                .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

            if (article == null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }
            return new ArticleEnvelope(article);
        }
    }
}
