namespace CoreApp.API.Features.Users;

public class UserResponse
{
    public string? Username { get; init; }

    public string? Email { get; init; }

    public string? Bio { get; init; }

    public string? Image { get; init; }

    public string? Token { get; set; }
}

