namespace BookHub.Tests.Shared.Identity;

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
    public const string NameClaim = "Administrator";
    public const string NameIdentifierClaim = "test-admin-id";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeaderIsNotProvided = !this
            .Request
            .Headers
            .ContainsKey("Authorization");

        if (authorizationHeaderIsNotProvided)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, NameIdentifierClaim),
            new(ClaimTypes.Name, NameClaim),
            new(ClaimTypes.Role, AdminRoleName),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
