using AutoMapper;
using CoreApp.API.Domain;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoreApp.API.Features.Users;

public class Edit
{
  public class UserData
  {
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Bio { get; set; }

    public string? Image { get; set; }
  }

  public record Command(UserData User) : ICommand<UserResponse>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator() => RuleFor(x => x.User).NotNull();
  }

  public class Handler(
      CoreAppContext context,
      ICurrentUserAccessor currentUserAccessor,
      IMapper mapper
  ) : ICommandHandler<Command, UserResponse>
  {
    public async ValueTask<UserResponse> Handle(Command message, CancellationToken cancellationToken)
    {
      var currentUsername = currentUserAccessor.GetCurrentUsername();
      var person = await context
          .Persons.Where(x => x.Username == currentUsername)
          .FirstOrDefaultAsync(cancellationToken);
      if (person is null)
      {
        throw new RestException(
            HttpStatusCode.NotFound,
            new { User = Constants.NOT_FOUND }
        );
      }

      person.Username = message.User.Username ?? person.Username;
      person.Email = message.User.Email ?? person.Email;
      person.Bio = message.User.Bio ?? person.Bio;
      person.Image = message.User.Image ?? person.Image;

      if (!string.IsNullOrWhiteSpace(message.User.Password))
      {
        var salt = Guid.NewGuid().ToByteArray();
      }

      await context.SaveChangesAsync(cancellationToken);

      var user = mapper.Map<Person, UserResponse>(person);

      return user;
    }
  }
}
