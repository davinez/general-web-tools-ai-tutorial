using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Articles;

public class Delete
{
    public record Command(string Slug) : IRequest;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(CoreAppContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command message, CancellationToken cancellationToken)
        {
            var article =
                await context.Articles.FirstOrDefaultAsync(
                    x => x.Slug == message.Slug,
                    cancellationToken
                )
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );

            context.Articles.Remove(article);
            await context.SaveChangesAsync(cancellationToken);
            await Task.FromResult(Unit.Value);
        }
    }
}
