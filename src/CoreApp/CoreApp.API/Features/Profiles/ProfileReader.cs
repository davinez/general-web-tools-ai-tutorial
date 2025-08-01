using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Profiles;

public class ProfileReader(
    CoreAppContext context,
    ICurrentUserAccessor currentUserAccessor,
    IMapper mapper
) : IProfileReader
{
    public async Task<ProfileEnvelope> ReadProfile(
        string username,
        CancellationToken cancellationToken
    )
    {
        var currentUserName = currentUserAccessor.GetCurrentUsername();

        var person = await context
            .Persons.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (person is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
        }

        if (person == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
        }
        var profile = mapper.Map<Domain.Person, Profile>(person);

        if (currentUserName != null)
        {
            var currentPerson = await context
                .Persons.Include(x => x.Following)
                .Include(x => x.Followers)
                .FirstOrDefaultAsync(x => x.Username == currentUserName, cancellationToken);

            if (currentPerson is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { User = Constants.NOT_FOUND }
                );
            }

            if (currentPerson.Followers.Any(x => x.TargetId == person.PersonId))
            {
                profile.IsFollowed = true;
            }
        }

        return new ProfileEnvelope(profile);
    }
}
