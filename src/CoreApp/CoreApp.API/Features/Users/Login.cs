using AutoMapper;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Security;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Features.Users;

public class Login
{
  public class UserData
  {
    public string? Email { get; init; }

    public string? Password { get; init; }
  }

  public record Command(UserData User) : IQuery<UserResponse>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.User).NotNull();
      RuleFor(x => x.User.Email).NotNull().NotEmpty();
      RuleFor(x => x.User.Password).NotNull().NotEmpty();
    }
  }

  public class Handler(
      CoreAppContext context,
      IJwtTokenGenerator jwtTokenGenerator,
      IMapper mapper
  ) : IQueryHandler<Command, UserResponse>
  {
    public async ValueTask<UserResponse> Handle(Command message, CancellationToken cancellationToken)
    {
      var person = await context
          .Persons.Where(x => x.Email == message.User.Email)
          .SingleOrDefaultAsync(cancellationToken);

      if (person == null)
      {
        throw new RestException(
            HttpStatusCode.Unauthorized,
            new { Error = "Invalid email / password." }
        );
      }

      var user = mapper.Map<Domain.Person, UserResponse>(person);
      user.Token = jwtTokenGenerator.CreateToken(
          person.Username ?? throw new InvalidOperationException()
      );

      return user;
    }
  }
}
