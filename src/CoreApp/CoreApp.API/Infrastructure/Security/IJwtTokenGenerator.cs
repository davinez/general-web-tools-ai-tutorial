namespace CoreApp.API.Infrastructure.Security;

public interface IJwtTokenGenerator
{
    string CreateToken(string username);
}
