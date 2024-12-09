namespace BookHub.Server.Features.Identity.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IIdentityService : ITransientService
    {
        Task<ResultWith<string>> RegisterAsync(string email, string username, string password);

        Task<ResultWith<string>> LoginAsync(string credentials, string password, bool rememberMe);
    }
}
