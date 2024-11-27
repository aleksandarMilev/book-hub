namespace BookHub.Server.Infrastructure.Services
{
    public interface ICurrentUserService
    {
        string? GetUsername();

        string? GetId();

        bool IsAdmin();
    }
}
