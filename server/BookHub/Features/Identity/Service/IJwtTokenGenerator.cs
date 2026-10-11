namespace BookHub.Features.Identity.Service;

using Infrastructure.Services.ServiceLifetimes;

public interface IJwtTokenGenerator : ISingletonService
{
    string Generate(
        string userId,
        string username,
        string email,
        bool rememberMe = false,
        bool isAdmin = false);
}
