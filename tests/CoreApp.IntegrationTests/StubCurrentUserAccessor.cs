using CoreApp.Infrastructure;

namespace CoreApp.IntegrationTests;

public class StubCurrentUserAccessor(string userName) : ICurrentUserAccessor
{
    public string GetCurrentUsername() => userName;
}
