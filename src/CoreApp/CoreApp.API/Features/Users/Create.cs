using AutoMapper;
using CoreApp.API.Domain;
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

public class Create
{
  public record UserData(string? Username, string? Email, string? Password);

  public record Command(UserData User) : ICommand<UserResponse>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.User.Username).NotNull().NotEmpty();
      RuleFor(x => x.User.Email).NotNull().NotEmpty();
      RuleFor(x => x.User.Password).NotNull().NotEmpty();
    }
  }

  public class Handler(
      CoreAppContext context,
      IJwtTokenGenerator jwtTokenGenerator,
      IMapper mapper
  ) : ICommandHandler<Command, UserResponse>
  {
    public async ValueTask<UserResponse> Handle(Command message, CancellationToken cancellationToken)
    {
      if (
          await context
              .Persons.Where(x => x.Username == message.User.Username)
              .AnyAsync(cancellationToken)
      )
      {
        throw new RestException(
            HttpStatusCode.BadRequest,
            new { Username = Constants.IN_USE }
        );
      }

      if (
          await context
              .Persons.Where(x => x.Email == message.User.Email)
              .AnyAsync(cancellationToken)
      )
      {
        throw new RestException(
            HttpStatusCode.BadRequest,
            new { Email = Constants.IN_USE }
        );
      }

      var salt = Guid.NewGuid().ToByteArray();
      var person = new Person
      {
        Username = message.User.Username,
        Email = message.User.Email,
      };

      await context.Persons.AddAsync(person, cancellationToken);
      await context.SaveChangesAsync(cancellationToken);

      var user = mapper.Map<Person, UserResponse>(person);
      user.Token = jwtTokenGenerator.CreateToken(
          person.Username ?? throw new InvalidOperationException()
      );
      return user;
    }
  }
}
