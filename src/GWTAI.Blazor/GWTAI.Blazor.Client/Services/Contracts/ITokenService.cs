namespace GWTAI.Blazor.Client.Services.Contracts;

public interface ITokenService
{
  Task SetTokenAsync(string token);

  Task<string?> GetTokenAsync();

  Task RemoveTokenAsync();

  Task<string?> GetUsernameFromExistingTokenAsync();
}
