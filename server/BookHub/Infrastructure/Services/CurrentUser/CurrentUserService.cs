namespace BookHub.Infrastructure.Services.CurrentUser;

using System.Security.Claims;
using Extensions;

using static Common.Constants.Names;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public string? GetUsername()
        => this.User?.Identity?.Name;

    public string? GetId()
        => this.User?.GetId();

    public bool IsAdmin()
        => this.User?.IsInRole(AdminRoleName) ?? false;
}
