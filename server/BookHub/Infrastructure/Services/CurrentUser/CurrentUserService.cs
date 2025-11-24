namespace BookHub.Infrastructure.Services.CurrentUser;

using System.Security.Claims;
using Extensions;

using static Common.Constants.Names;

public class CurrentUserService(IHttpContextAccessor httpContext) : ICurrentUserService
{
    private readonly ClaimsPrincipal user = httpContext.HttpContext?.User!;

    public string? GetUsername()
        => user?.Identity?.Name;

    public string? GetId()
        => user?.GetId();

    public bool IsAdmin()
        => user.IsInRole(AdminRoleName);
}
