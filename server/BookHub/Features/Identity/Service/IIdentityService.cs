namespace BookHub.Features.Identity.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IIdentityService : ITransientService
{
    Task<ResultWith<string>> Register(
        RegisterServiceModel model,
        CancellationToken cancellationToken = default);

    Task<ResultWith<string>> Login(
        LoginServiceModel model,
        CancellationToken cancellationToken = default);

    Task<ResultWith<string>> ForgotPassword(
        ForgotPasswordServiceModel model,
        CancellationToken cancellationToken = default);

    Task<ResultWith<string>> ResetPassword(
        ResetPasswordServiceModel model,
        CancellationToken cancellationToken = default);
}
