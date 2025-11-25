namespace BookHub.Features.Identity.Service;

using Infrastructure.Services.Result;
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
