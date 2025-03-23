namespace BookHub.Infrastructure.Services
{
    using ServiceLifetimes;

    public interface ICurrentUserService : IScopedService
    {
        string? GetUsername();

        string? GetId();

        bool IsAdmin();
    }
}
