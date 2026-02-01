namespace BookHub.Tests.Shared.Mocks;

using BookHub.Infrastructure.Services.CurrentUser;

public sealed class CurrentUserServiceMock(
    string id = "42",
    string username = "shano",
    bool isAdmin = false) : ICurrentUserService
{
    public string? GetUsername()
        => username;

    public string? GetId()
        => id;

    public bool IsAdmin()
        => isAdmin;
}
