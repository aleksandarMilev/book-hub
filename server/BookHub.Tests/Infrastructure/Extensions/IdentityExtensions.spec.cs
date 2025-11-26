namespace BookHub.Tests.Infrastructure.Extensions;

using System.Security.Claims;
using BookHub.Infrastructure.Extensions;
using Xunit;

public class IdentityExtensionsTests
{
    [Fact]
    public void GetId_ReturnsNameIdentifierClaimValue_WhenPresent()
    {
        const string userId = "12345";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, "test-user")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.GetId();

        Assert.Equal(userId, result);
    }

    [Fact]
    public void GetId_ThrowsInvalidOperationException_WhenNameIdentifierMissing()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "test-user")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var exception = Assert.Throws<InvalidOperationException>(() => principal.GetId());

        Assert.Equal("User Id not found!", exception.Message);
    }
}
