namespace BookHub.Server.Infrastructure.Services
{
    using System.Security.Claims;

    using Extensions;

    public class CurrentUserService(IHttpContextAccessor httpContext) : ICurrentUserService
    {
        private readonly ClaimsPrincipal user = httpContext.HttpContext?.User!;

        public string? GetUsername()
            => this.user?.Identity?.Name;

        public string? GetId()
            => this.user?.GetId();
    }
}
