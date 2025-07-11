using System.Linq;
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

namespace CoreApp.API.Features.Comments;

public class Delete
{
    public record Command(string Slug, int Id) : IRequest;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(CoreAppContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command message, CancellationToken cancellationToken)
        {
            var article =
                await context
                    .Articles.Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken)
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );

            var comment =
                article.Comments.FirstOrDefault(x => x.CommentId == message.Id)
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Comment = Constants.NOT_FOUND }
                );

            context.Comments.Remove(comment);
            await context.SaveChangesAsync(cancellationToken);
            await Task.FromResult(Unit.Value);
        }
    }
}
