using System.Threading.Tasks;
using CoreApp.API.Features.Users;

namespace CoreApp.IntegrationTests.Features.Users;

public static class UserHelpers
{
  public static readonly string DefaultUserName = "username";

  /// <summary>
  /// creates a default user to be used in different tests
  /// </summary>
  /// <param name="fixture"></param>
  /// <returns></returns>
  public static async Task<UserResponse> CreateDefaultUser(SliceFixture fixture)
  {
    var command = new Create.Command(new Create.UserData(DefaultUserName, "email", "password"));

    var commandResult = await fixture.SendAsync(command);
    return commandResult.User;
  }
}
