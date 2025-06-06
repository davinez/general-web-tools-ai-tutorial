using System.Linq;
using System.Threading.Tasks;
using CoreApp.API.Features.Users;
using CoreApp.API.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreApp.IntegrationTests.Features.Users;

public class CreateTests : SliceFixture
{
  [Fact]
  public async Task Expect_Create_User()
  {
    var command = new Create.Command(new Create.UserData("username", "email", "password"));

    await SendAsync(command);

    var created = await ExecuteDbContextAsync(db =>
        db.Persons.Where(d => d.Email == command.User.Email).SingleOrDefaultAsync()
    );

    Assert.NotNull(created);
    Assert.Equal(created.Hash, await new PasswordHasher().Hash("password", created.Salt));
  }
}
