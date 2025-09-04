namespace CoreApp.API.Domain.Security;

public interface ICurrentUserAccessor
{
    string GetCurrentUsername();
}
