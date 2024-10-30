namespace BookHub.Server.Infrastructure.Services
{
    using System.Security.Claims;

    using BookHub.Server.Infrastructure.Extensions;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContext)
            => this.user = httpContext.HttpContext?.User!;

        public string? GetUsername()
            => this.user?.Identity?.Name;

        public string? GetId()
            => this.user?.GetId();
    }
}
