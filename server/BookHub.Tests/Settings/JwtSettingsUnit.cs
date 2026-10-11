namespace BookHub.Tests.Settings;

using FluentAssertions;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

// JwtSettings is validated on startup through ValidateDataAnnotations().ValidateOnStart().
// These tests run the same validator, so a failure here is a startup failure there.
public sealed class JwtSettingsUnit
{
    private const string ValidSecret = "a-valid-test-secret-that-is-at-least-32-bytes";

    [Fact]
    public void Validate_ShouldSucceed_WhenEverySettingIsValid()
    {
        var result = Validate(new JwtSettings
        {
            Secret = ValidSecret,
            Issuer = "BookHub",
            Audience = "BookHubClient",
        });

        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldFailWithAClearMessage_WhenTheIssuerIsMissingOrBlank(string? issuer)
    {
        var result = Validate(new JwtSettings
        {
            Secret = ValidSecret,
            Issuer = issuer!,
            Audience = "BookHubClient",
        });

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("JwtSettings:Issuer is required");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldFailWithAClearMessage_WhenTheAudienceIsMissingOrBlank(string? audience)
    {
        var result = Validate(new JwtSettings
        {
            Secret = ValidSecret,
            Issuer = "BookHub",
            Audience = audience!,
        });

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("JwtSettings:Audience is required");
    }

    [Fact]
    public void Validate_ShouldFail_WhenTheSecretIsShorterThan32Bytes()
    {
        var result = Validate(new JwtSettings
        {
            Secret = "too-short",
            Issuer = "BookHub",
            Audience = "BookHubClient",
        });

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("at least 32 bytes");
    }

    private static ValidateOptionsResult Validate(JwtSettings settings)
        => new DataAnnotationValidateOptions<JwtSettings>(Options.DefaultName)
            .Validate(Options.DefaultName, settings);
}
