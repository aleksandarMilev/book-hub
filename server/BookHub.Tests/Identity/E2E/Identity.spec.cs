namespace BookHub.Tests.Identity.E2E;

using System.IdentityModel.Tokens.Jwt;
using Features.Email;
using Features.Identity.Data.Models;
using Features.Identity.Service;
using Features.Identity.Web;
using Features.Identity.Web.Models;
using Features.UserProfile.Service;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using BookHub.Tests.Helpers;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class IdentityFeatureE2eTests
{
    [Fact]
    public async Task Register_then_login_happy_path_returns_jwts_and_triggers_side_effects()
    {
        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager
            .Setup(m => m.CreateAsync(It.IsAny<UserDbModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<UserDbModel, string>((u, _) => u.Id = "e2e-user");

        userManager.Setup(m => m.FindByNameAsync("e2e")).ReturnsAsync(new UserDbModel
        {
            Id = "e2e-user",
            UserName = "e2e",
            Email = "e2e@example.com"
        });

        userManager.Setup(
            m => m.IsLockedOutAsync(It.IsAny<UserDbModel>())).ReturnsAsync(false);

        userManager.Setup(
            m => m.CheckPasswordAsync(It.IsAny<UserDbModel>(), "Passw0rd!")).ReturnsAsync(true);

        userManager.Setup(
            m => m.ResetAccessFailedCountAsync(It.IsAny<UserDbModel>())).ReturnsAsync(IdentityResult.Success);

        userManager.Setup(
            m => m.IsInRoleAsync(It.IsAny<UserDbModel>(), It.IsAny<string>())).ReturnsAsync(false);

        var emailSender = new Mock<IEmailSender>();
        var profileService = new Mock<IProfileService>();
        var logger = new Mock<ILogger<IdentityService>>();

        var service = new IdentityService(
            userManager.Object,
            emailSender.Object,
            profileService.Object,
            logger.Object,
            JwtOptions());

        var controller = new IdentityController(service);

        var registerResult = await controller.Register(new RegisterWebModel
        {
            Username = "e2e",
            Email = "e2e@example.com",
            Password = "Passw0rd!",
            FirstName = "E",
            LastName = "TwoE",
            Biography = "bio",
            IsPrivate = false
        }, CancellationToken.None);

        registerResult.GetStatusCode().Should().Be(200);

        var registerPayload = registerResult.GetValue();
        registerPayload.Should().NotBeNull();
        registerPayload!.Token.Should().NotBeNullOrWhiteSpace();

        profileService.Verify(
            p => p.Create(
                It.IsAny<CreateProfileServiceModel>(),
                "e2e-user",
                It.IsAny<CancellationToken>()),
            Times.Once);

        emailSender.Verify(
            e => e.SendWelcome("e2e@example.com", "e2e"),
            Times.Once);

        var loginResult = await controller.Login(new LoginWebModel
        {
            Credentials = "e2e",
            Password = "Passw0rd!",
            RememberMe = false
        }, CancellationToken.None);

        loginResult.GetStatusCode().Should().Be(200);

        var loginPayload = loginResult.GetValue();
        loginPayload.Should().NotBeNull();
        loginPayload!.Token.Should().NotBeNullOrWhiteSpace();

        var token = new JwtSecurityTokenHandler().ReadJwtToken(loginPayload.Token);
        token
            .Claims
            .Should()
            .Contain(
                c => c.Type.EndsWith("/nameidentifier") && c.Value == "e2e-user");

        token
            .Claims
            .Should()
            .Contain(
                c => c.Type.EndsWith("/name") && c.Value == "e2e");

        token
            .Claims
            .Should()
            .Contain(
                c => c.Type.EndsWith("/emailaddress") && c.Value == "e2e@example.com");
    }

    private static IOptions<JwtSettings> JwtOptions()
        => Options.Create(new JwtSettings
        {
            Secret = "THIS_IS_A_TEST_SECRET_32_CHARS_MINIMUM!!",
            Issuer = "BookHub.Tests",
            Audience = "BookHub.Tests"
        });
}
