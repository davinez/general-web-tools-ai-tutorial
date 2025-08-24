using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CoreApp.API.Infrastructure;

//public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
public class CurrentUserAccessor() : ICurrentUserAccessor
{
  // Force to always return a string if not then user is not authenticated
  public string GetCurrentUsername() => "testUsernameGuid";
  //public string? GetCurrentUsername() =>
  //      httpContextAccessor
  //          .HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
  //          ?.Value;
}
