namespace BookHub.Tests.Infrastructure.Services.CurrentUser;

using BookHub.Infrastructure.Services.CurrentUser;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Xunit;

using static BookHub.Common.Constants.Names;

public class CurrentUserServiceTests
{
    [Fact]
    public void GetUsername_ReturnsName_WhenUserIsPresent()
    {
        var httpContextAccessor = CreateHttpContextAccessorWithUser(
            username: "test-user",
            userId: "user-123",
            isAdmin: false);

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.GetUsername();

        Assert.Equal("test-user", result);
    }

    [Fact]
    public void GetUsername_ReturnsNull_WhenNoUser()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };
        httpContextAccessor.HttpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity());

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.GetUsername();

        Assert.Null(result);
    }

    [Fact]
    public void GetId_ReturnsId_WhenClaimIsPresent()
    {
        var httpContextAccessor = CreateHttpContextAccessorWithUser(
            username: "user",
            userId: "user-123",
            isAdmin: false);

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.GetId();

        Assert.Equal("user-123", result);
    }

    [Fact]
    public void GetId_ReturnsNull_WhenNoUser()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = null
        };

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.GetId();

        Assert.Null(result);
    }

    [Fact]
    public void IsAdmin_ReturnsTrue_WhenUserInAdminRole()
    {
        var httpContextAccessor = CreateHttpContextAccessorWithUser(
            username: "admin-user",
            userId: "admin-1",
            isAdmin: true);

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.IsAdmin();

        Assert.True(result);
    }

    [Fact]
    public void IsAdmin_ReturnsFalse_WhenUserNotInAdminRole()
    {
        var httpContextAccessor = CreateHttpContextAccessorWithUser(
            username: "regular-user",
            userId: "user-1",
            isAdmin: false);

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.IsAdmin();

        Assert.False(result);
    }

    [Fact]
    public void IsAdmin_ReturnsFalse_WhenNoUser()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = null
        };

        var service = new CurrentUserService(httpContextAccessor);

        var result = service.IsAdmin();

        Assert.False(result);
    }

    private static IHttpContextAccessor CreateHttpContextAccessorWithUser(
        string username,
        string userId,
        bool isAdmin)
    {
        var identity = new ClaimsIdentity("TestAuthType");
        identity.AddClaim(new Claim(ClaimTypes.Name, username));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

        if (isAdmin)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, AdminRoleName));
        }

        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext
        {
            User = principal
        };

        return new HttpContextAccessor
        {
            HttpContext = context
        };
    }
}
