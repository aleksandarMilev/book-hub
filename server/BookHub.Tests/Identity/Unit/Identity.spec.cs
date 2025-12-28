namespace BookHub.Tests.Identity.Unit;

using System.IdentityModel.Tokens.Jwt;
using Features.Email;
using Features.Identity.Data.Models;
using Features.Identity.Service;
using Features.Identity.Service.Models;
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

using static Features.Identity.Shared.Constants.ErrorMessages;

public class IdentityServiceTests
{
    [Fact]
    public async Task Register_should_return_success_jwt_and_call_profile_and_email_when_user_created()
    {
        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager
            .Setup(m => m.CreateAsync(It.IsAny<UserDbModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<UserDbModel, string>((u, _) => u.Id = "user-1");

        var emailSender = new Mock<IEmailSender>();
        var profileService = new Mock<IProfileService>();
        var logger = new Mock<ILogger<IdentityService>>();

        var service = new IdentityService(
            userManager.Object,
            emailSender.Object,
            profileService.Object,
            logger.Object,
            JwtOptions());

        var model = new RegisterServiceModel
        {
            Username = "sashko",
            Email = "sashko@example.com",
            Password = "Passw0rd!",
            FirstName = "Sashko",
            LastName = "Test",
            Biography = "bio",
            DateOfBirth = "1990-01-01",
            IsPrivate = false,
            SocialMediaUrl = "https://example.com"
        };

        var result = await service.Register(model, CancellationToken.None);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeTrue();

        ResultWithReflection
            .GetValue<string>(result)
            .Should()
            .NotBeNullOrWhiteSpace();

        profileService.Verify(
            p => p.Create(
                It.IsAny<CreateProfileServiceModel>(),
                "user-1",
                It.IsAny<CancellationToken>()),
            Times.Once,
            "profile creation should happen after successful user creation");

        emailSender.Verify(
            e => e.SendWelcome(model.Email, model.Username),
            Times.Once);

        userManager.Verify(
            m => m.DeleteAsync(It.IsAny<UserDbModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_should_delete_user_and_return_failure_when_downstream_dependency_throws()
    {
        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager
            .Setup(m => m.CreateAsync(It.IsAny<UserDbModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<UserDbModel, string>((u, _) => u.Id = "user-2");

        var emailSender = new Mock<IEmailSender>();
        var profileService = new Mock<IProfileService>();
        profileService
            .Setup(
                p => p.Create(It.IsAny<CreateProfileServiceModel>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var logger = new Mock<ILogger<IdentityService>>();

        var service = new IdentityService(
            userManager.Object,
            emailSender.Object,
            profileService.Object,
            logger.Object,
            JwtOptions());

        var model = new RegisterServiceModel
        {
            Username = "failcase",
            Email = "fail@example.com",
            Password = "Passw0rd!",
            FirstName = "Fail",
            LastName = "Case"
        };

        var result = await service.Register(model, CancellationToken.None);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeFalse();

        ResultWithReflection
            .GetError(result)
            .Should()
            .Contain(InvalidRegisterAttempt);

        userManager.Verify(
            m => m.DeleteAsync(It.IsAny<UserDbModel>()), Times.Once);

        emailSender.Verify(
            e => e.SendWelcome(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_should_return_failure_with_identity_errors_when_create_fails()
    {
        var userManager = UserManagerMockFactory.Create<UserDbModel>();
        var identityError = new IdentityError
        { 
            Description = "Password too weak"
        };

        userManager
            .Setup(m => m.CreateAsync(It.IsAny<UserDbModel>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var service = new IdentityService(
            userManager.Object,
            new Mock<IEmailSender>().Object,
            new Mock<IProfileService>().Object,
            new Mock<ILogger<IdentityService>>().Object,
            JwtOptions());

        var serviceModel = new RegisterServiceModel
        {
            Username = "user",
            Email = "user@example.com",
            Password = "bad",
            FirstName = "A",
            LastName = "B"
        };

        var result = await service.Register(serviceModel);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeFalse();

        ResultWithReflection
            .GetError(result)
            .Should()
            .Contain("Password too weak");
    }

    [Fact]
    public async Task Login_should_return_failure_when_user_not_found_by_username_or_email()
    {
        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager
            .Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((UserDbModel?)null);

        userManager
            .Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((UserDbModel?)null);

        var service = new IdentityService(
            userManager.Object,
            new Mock<IEmailSender>().Object,
            new Mock<IProfileService>().Object,
            new Mock<ILogger<IdentityService>>().Object,
            JwtOptions());

        var serviceModel = new LoginServiceModel
        {
            Credentials = "missing",
            Password = "whatever"
        };

        var result = await service.Login(serviceModel);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeFalse();

        ResultWithReflection
            .GetError(result)
            .Should()
            .Contain(InvalidLoginAttempt);
    }

    [Fact]
    public async Task Login_should_return_failure_when_account_is_locked()
    {
        var user = new UserDbModel
        { 
            Id = "u1",
            UserName = "locked",
            Email = "locked@example.com"
        };

        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager.Setup(m => m.FindByNameAsync("locked")).ReturnsAsync(user);
        userManager.Setup(m => m.IsLockedOutAsync(user)).ReturnsAsync(true);

        var service = new IdentityService(
            userManager.Object,
            new Mock<IEmailSender>().Object,
            new Mock<IProfileService>().Object,
            new Mock<ILogger<IdentityService>>().Object,
            JwtOptions());

        var serviceModel = new LoginServiceModel
        {
            Credentials = "locked",
            Password = "x"
        };

        var result = await service.Login(serviceModel);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeFalse();

        ResultWithReflection
            .GetError(result)
            .Should()
            .Contain(AccountIsLocked);

        userManager
            .Verify(m => m.CheckPasswordAsync(
                It.IsAny<UserDbModel>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Login_should_return_success_and_reset_failed_count_when_password_valid()
    {
        var user = new UserDbModel
        { 
            Id = "u2",
            UserName = "ok",
            Email = "ok@example.com"
        };

        var userManager = UserManagerMockFactory.Create<UserDbModel>();

        userManager
            .Setup(m => m.FindByNameAsync("ok"))
            .ReturnsAsync(user);

        userManager
            .Setup(m => m.IsLockedOutAsync(user))
            .ReturnsAsync(false);

        userManager
            .Setup(m => m.CheckPasswordAsync(user, "Passw0rd!"))
            .ReturnsAsync(true);

        userManager
            .Setup(m => m.ResetAccessFailedCountAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        userManager
            .Setup(m => m.IsInRoleAsync(user, It.IsAny<string>()))
            .ReturnsAsync(true);

        var sut = new IdentityService(
            userManager.Object,
            new Mock<IEmailSender>().Object,
            new Mock<IProfileService>().Object,
            new Mock<ILogger<IdentityService>>().Object,
            JwtOptions());

        var serviceModel = new LoginServiceModel
        {
            Credentials = "ok",
            Password = "Passw0rd!",
            RememberMe = true
        };

        var result = await sut.Login(serviceModel);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeTrue();

        var jwt = ResultWithReflection.GetValue<string>(result);
        jwt.Should().NotBeNullOrWhiteSpace();

        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        token
            .Claims
            .Should()
            .Contain(c => c.Type.EndsWith("/nameidentifier") && c.Value == "u2");

        token
            .Claims
            .Should()
            .Contain(c => c.Type.EndsWith("/name") && c.Value == "ok");

        token
            .Claims
            .Should()
            .Contain(c => c.Type.EndsWith("/emailaddress") && c.Value == "ok@example.com");

        token
            .Claims
            .Should()
            .Contain(c => c.Type.EndsWith("/role"));

        token
            .ValidTo
            .Should().BeAfter(DateTime.UtcNow.AddDays(14));

        userManager.Verify(
            m => m.ResetAccessFailedCountAsync(user),
            Times.Once);

        userManager.Verify(
            m => m.AccessFailedAsync(It.IsAny<UserDbModel>()),
            Times.Never);
    }

    [Fact]
    public async Task Login_should_increment_failed_count_and_return_locked_message_when_lockout_happens_after_failed_password()
    {
        var user = new UserDbModel
        { 
            Id = "u3",
            UserName = "bad",
            Email = "bad@example.com"
        };

        var userManager = UserManagerMockFactory.Create<UserDbModel>();
        userManager
            .Setup(m => m.FindByNameAsync("bad"))
            .ReturnsAsync(user);

        userManager
            .SetupSequence(m => m.IsLockedOutAsync(user))
            .ReturnsAsync(false)
            .ReturnsAsync(true);

        userManager
            .Setup(m => m.CheckPasswordAsync(user, "wrong"))
            .ReturnsAsync(false);

        userManager
            .Setup(m => m.AccessFailedAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var service = new IdentityService(
            userManager.Object,
            new Mock<IEmailSender>().Object,
            new Mock<IProfileService>().Object,
            new Mock<ILogger<IdentityService>>().Object,
            JwtOptions());

        var serviceModel = new LoginServiceModel
        {
            Credentials = "bad",
            Password = "wrong"
        };

        var result = await service.Login(serviceModel);

        ResultWithReflection
            .IsSuccess(result)
            .Should()
            .BeFalse();

        ResultWithReflection
            .GetError(result)
            .Should()
            .Contain(AccountWasLocked);

        userManager.Verify(
            m => m.AccessFailedAsync(user),
            Times.Once);

        userManager.Verify(
            m => m.ResetAccessFailedCountAsync(It.IsAny<UserDbModel>()),
            Times.Never);
    }

    private static IOptions<JwtSettings> JwtOptions()
       => Options.Create(new JwtSettings
       {
           Secret = "THIS_IS_A_TEST_SECRET_32_CHARS_MINIMUM!!",
           Issuer = "BookHub.Tests",
           Audience = "BookHub.Tests"
       });
}
