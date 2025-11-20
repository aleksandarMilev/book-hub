namespace BookHub.Features.Identity.Service
{
    using BookHub.Infrastructure.Services.Result;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IIdentityService : ITransientService
    {
        Task<ResultWith<string>> Register(
            string email,
            string username,
            string password);

        Task<ResultWith<string>> Login(
            string credentials,
            string password,
            bool rememberMe);
    }
}
