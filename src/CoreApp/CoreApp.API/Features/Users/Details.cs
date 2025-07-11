using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Errors;
using CoreApp.API.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Users;

public class Details
{
    public record Query(string Username) : IRequest<UserEnvelope>;

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => RuleFor(x => x.Username).NotNull().NotEmpty();
    }

    public class QueryHandler(
        CoreAppContext context,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper
    ) : IRequestHandler<Query, UserEnvelope>
    {
        public async Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
        {
            var person = await context
                .Persons.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

            if (person == null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { User = Constants.NOT_FOUND }
                );
            }

            var user = mapper.Map<Domain.Person, User>(person);
            user.Token = jwtTokenGenerator.CreateToken(
                person.Username ?? throw new InvalidOperationException()
            );
            return new UserEnvelope(user);
        }
    }
}
