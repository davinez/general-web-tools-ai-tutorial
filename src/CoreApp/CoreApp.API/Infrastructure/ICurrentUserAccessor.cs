namespace CoreApp.API.Infrastructure;

public interface ICurrentUserAccessor
{
    string GetCurrentUsername();
}
