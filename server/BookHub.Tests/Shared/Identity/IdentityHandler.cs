namespace BookHub.Tests.Shared.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using static BookHub.Common.Constants.Names;

public sealed class IdentityHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(
        options,
        logger,
        encoder)
{
    public const string SchemeName = "TestSchema";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeaderNotProvided = !this
            .Request
            .Headers
            .TryGetValue("Authorization", out var rawAuthHeader);

        if (authHeaderNotProvided)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authHeaderIsInvalid = !AuthenticationHeaderValue
            .TryParse(rawAuthHeader, out var parsedHeader);

        if (authHeaderIsInvalid || parsedHeader is null)
        {
            return Task.FromResult(
                AuthenticateResult.Fail("Invalid Authorization header"));
        }

        if (!string.Equals(parsedHeader.Scheme, SchemeName, StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var token = parsedHeader.Parameter ?? "";
        var parts = token.Split(':', 3, StringSplitOptions.RemoveEmptyEntries);

        var kind = parts.Length > 0 ? parts[0] : "user";
        var userId = parts.Length > 1 ? parts[1] : "test-user";
        var username = parts.Length > 2 ? parts[2] : "user";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),
        };

        var isAdmin = string.Equals(kind, "admin", StringComparison.OrdinalIgnoreCase);
        if (isAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, AdminRoleName));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
