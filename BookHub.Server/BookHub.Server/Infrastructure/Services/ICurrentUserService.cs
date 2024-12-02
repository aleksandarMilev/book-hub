namespace BookHub.Server.Infrastructure.Services
{
    using Infrastructure.Services.ServiceLifetimes;

    public interface ICurrentUserService : IScopedService
    {
        string? GetUsername();

        string? GetId();

        bool IsAdmin();
    }
}
