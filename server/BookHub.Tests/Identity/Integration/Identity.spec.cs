namespace BookHub.Tests.Identity.Integration;

using Features.Identity.Service;
using Features.Identity.Service.Models;
using Features.Identity.Web;
using Features.Identity.Web.Models;
using FluentAssertions;
using BookHub.Tests.Helpers;
using Moq;
using Xunit;

public class IdentityControllerTests
{
    [Fact]
    public async Task Register_should_call_service_with_mapped_model_and_return_ok_when_success()
    {
        var service = new Mock<IIdentityService>();

        service.Setup(s => s.Register(
                It.Is<RegisterServiceModel>(m =>
                    m.Username == "u" &&
                    m.Email == "u@example.com" &&
                    m.Password == "Passw0rd!" &&
                    m.FirstName == "First" &&
                    m.LastName == "Last" &&
                    m.Biography == "Bio" &&
                    m.DateOfBirth == "1990-01-01" &&
                    m.IsPrivate == true &&
                    m.SocialMediaUrl == "https://example.com"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultWithFactory.Success("jwt-token"));

        var controller = new IdentityController(service.Object);

        var result = await controller.Register(new RegisterWebModel
        {
            Username = "u",
            Email = "u@example.com",
            Password = "Passw0rd!",
            FirstName = "First",
            LastName = "Last",
            Biography = "Bio",
            DateOfBirth = "1990-01-01",
            IsPrivate = true,
            SocialMediaUrl = "https://example.com"
        });

        result.GetStatusCode().Should().Be(200);

        var payload = result.GetValue();
        payload.Should().NotBeNull();
        payload!.Token.Should().Be("jwt-token");

        service.VerifyAll();
    }

    [Fact]
    public async Task Register_should_return_bad_request_when_service_returns_failure()
    {
        var service = new Mock<IIdentityService>();

        service
            .Setup(
                s => s.Register(It.IsAny<RegisterServiceModel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultWithFactory.Failure<string>("nope"));

        var controller = new IdentityController(service.Object);

        var result = await controller.Register(new RegisterWebModel
        {
            Username = "u",
            Email = "u@example.com",
            Password = "Passw0rd!",
            FirstName = "First",
            LastName = "Last",
        });

        result.GetStatusCode().Should().Be(400);
    }

    [Fact]
    public async Task Login_should_call_service_with_mapped_model_and_return_ok_when_success()
    {
        var service = new Mock<IIdentityService>();
        service.Setup(s => s.Login(
                It.Is<LoginServiceModel>(m =>
                    m.Credentials == "creds" &&
                    m.Password == "Passw0rd!" &&
                    m.RememberMe == true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultWithFactory.Success("jwt"));

        var controller = new IdentityController(service.Object);

        var result = await controller.Login(new LoginWebModel
        {
            Credentials = "creds",
            Password = "Passw0rd!",
            RememberMe = true
        });

        result.GetStatusCode().Should().Be(200);

        var payload = result.GetValue();
        payload.Should().NotBeNull();
        payload!.Token.Should().Be("jwt");

        service.VerifyAll();
    }

    [Fact]
    public async Task Login_should_return_bad_request_when_service_returns_failure()
    {
        var service = new Mock<IIdentityService>();

        service
            .Setup(
                s => s.Login(It.IsAny<LoginServiceModel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultWithFactory.Failure<string>("bad login"));

        var controller = new IdentityController(service.Object);

        var result = await controller.Login(new LoginWebModel
        {
            Credentials = "creds",
            Password = "wrong",
            RememberMe = false
        });

        result.GetStatusCode().Should().Be(400);
    }
}
