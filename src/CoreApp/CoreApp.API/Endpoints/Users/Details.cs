using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Security;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Endpoints.Users;

public class Details
{
  public record Query(string Username) : IQuery<UserResponse>;

  public class QueryValidator : AbstractValidator<Query>
  {
    public QueryValidator() => RuleFor(x => x.Username).NotNull().NotEmpty();
  }

  public class QueryHandler(
      CoreAppContext context,
      IJwtTokenGenerator jwtTokenGenerator
  ) : IQueryHandler<Query, UserResponse>
  {
    public async ValueTask<UserResponse> Handle(Query message, CancellationToken cancellationToken)
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

      // PENDING
      var user = new UserResponse()
      {
        Bio = person.Bio,
        Username = person.Username,
      };

      user.Token = jwtTokenGenerator.CreateToken(
          person.Username ?? throw new InvalidOperationException()
      );

      return user;
    }
  }
}
