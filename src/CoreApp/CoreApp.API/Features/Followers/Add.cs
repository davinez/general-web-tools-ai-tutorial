using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Features.Profiles;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Followers;

public class Add
{
    public record Command(string Username) : IRequest<ProfileEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Username).NotNull().NotEmpty();
    }

    public class QueryHandler(
        CoreAppContext context,
        ICurrentUserAccessor currentUserAccessor,
        IProfileReader profileReader
    ) : IRequestHandler<Command, ProfileEnvelope>
    {
        public async Task<ProfileEnvelope> Handle(
            Command message,
            CancellationToken cancellationToken
        )
        {
            var target = await context.Persons.FirstOrDefaultAsync(
                x => x.Username == message.Username,
                cancellationToken
            );

            if (target is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { User = Constants.NOT_FOUND }
                );
            }

            var observer = await context.Persons.FirstOrDefaultAsync(
                x => x.Username == currentUserAccessor.GetCurrentUsername(),
                cancellationToken
            );

            if (observer is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { User = Constants.NOT_FOUND }
                );
            }

            var followedPeople = await context.FollowedPeople.FirstOrDefaultAsync(
                x => x.ObserverId == observer.PersonId && x.TargetId == target.PersonId,
                cancellationToken
            );

            if (followedPeople == null)
            {
                followedPeople = new FollowedPeople
                {
                    Observer = observer,
                    ObserverId = observer.PersonId,
                    Target = target,
                    TargetId = target.PersonId
                };
                await context.FollowedPeople.AddAsync(followedPeople, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            return await profileReader.ReadProfile(message.Username, cancellationToken);
        }
    }
}
