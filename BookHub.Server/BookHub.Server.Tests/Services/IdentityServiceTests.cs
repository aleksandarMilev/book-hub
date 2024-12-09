namespace BookHub.Server.Tests.Services
{
    using Features.Identity.Data.Models;
    using Features.Identity.Service;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class IdentityServiceTests
    {
        private readonly Mock<UserManager<User>> mockUserManager;
        private readonly Mock<IOptions<AppSettings>> mockAppSettings;

        private readonly IdentityService identityService;

        public IdentityServiceTests()
        {
            this.mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null!, null!, null!, null!, null!, null!, null!, null!
            );

            this.mockAppSettings = new Mock<IOptions<AppSettings>>();

            var appSettings = new AppSettings { Secret = "super-mega-giga-ultra-secret-key" };
            this.mockAppSettings
                .Setup(x => x.Value)
                .Returns(appSettings);

            this.identityService = new IdentityService(
                this.mockUserManager.Object, 
                this.mockAppSettings.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsCreatedSuccessfully()
        {
            var username = "my-user";
            var email = "my-user@mail.com";
            var password = "123456";

            var user = new User()
            { 
                Email = email,
                UserName = username 
            };

            this.mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await this.identityService.RegisterAsync(email, username, password);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenUserCreationFails()
        {
            var username = "my-user";
            var email = "my-user@mail.com";
            var password = "123456";

            var user = new User()
            {
                Email = email,
                UserName = username
            };

            var identityError = new IdentityError() { Description = "User already exists" };
            var identityResult = IdentityResult.Failed(identityError);

            this.mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), password))
                .ReturnsAsync(identityResult);

            var result = await this.identityService.RegisterAsync(email, username, password);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("User already exists");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccess_WhenUsernameIsValid()
        {
            var credentials = "my-user";
            var password = "123456";
            var rememberMe = true;

            var user = new User()
            { 
                UserName = credentials,
                Email = "test@mail.com" 
            };

            this.mockUserManager
                .Setup(x => x.FindByNameAsync(credentials))
                .ReturnsAsync(user);

            this.mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);

            var result = await this.identityService.LoginAsync(credentials, password, rememberMe);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccess_WhenEmailIsValid()
        {
            var credentials = "my-user@mail.com";
            var password = "123456";
            var rememberMe = true;

            var user = new User()
            {
                Email = credentials,
                UserName = "test"
            };

            this.mockUserManager
                .Setup(x => x.FindByEmailAsync(credentials))
                .ReturnsAsync(user);

            this.mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);

            var result = await this.identityService.LoginAsync(credentials, password, rememberMe);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            var credentials = "invalid";
            var password = "123456";
            var rememberMe = true;

            this.mockUserManager
                .Setup(x => x.FindByNameAsync(credentials))
                .ReturnsAsync((User)null!);

            var result = await this.identityService.LoginAsync(credentials, password, rememberMe);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid log in attempt!");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            var credentials = "my-user@mail.com";
            var password = "invalid";
            var rememberMe = true;

            var user = new User()
            { 
                UserName = credentials,
                Email = "test@mail.com"
            };

            this.mockUserManager
                .Setup(x => x.FindByNameAsync(credentials))
                .ReturnsAsync(user);

            this.mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, password))
                .ReturnsAsync(false);

            var result = await this.identityService.LoginAsync(credentials, password, rememberMe);

            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be("Invalid log in attempt!");
        }
    }
}
