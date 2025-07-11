using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Features.Articles;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Favorites;

public class Add
{
    public record Command(string Slug) : IRequest<ArticleEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor)
        : IRequestHandler<Command, ArticleEnvelope>
    {
        public async Task<ArticleEnvelope> Handle(
            Command message,
            CancellationToken cancellationToken
        )
        {
            var article = await context.Articles.FirstOrDefaultAsync(
                x => x.Slug == message.Slug,
                cancellationToken
            );

            if (article == null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }

            var person = await context.Persons.FirstOrDefaultAsync(
                x => x.Username == currentUserAccessor.GetCurrentUsername(),
                cancellationToken
            );

            if (person is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }

            var favorite = await context.ArticleFavorites.FirstOrDefaultAsync(
                x => x.ArticleId == article.ArticleId && x.PersonId == person.PersonId,
                cancellationToken
            );

            if (favorite == null)
            {
                favorite = new ArticleFavorite
                {
                    Article = article,
                    ArticleId = article.ArticleId,
                    Person = person,
                    PersonId = person.PersonId
                };
                await context.ArticleFavorites.AddAsync(favorite, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            article = await context
                .Articles.GetAllData()
                .FirstOrDefaultAsync(x => x.ArticleId == article.ArticleId, cancellationToken);
            if (article is null)
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
