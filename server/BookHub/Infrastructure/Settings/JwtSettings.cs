namespace BookHub.Infrastructure.Settings;

public class JwtSettings
{
    public JwtSettings()
        => this.Secret = default!;

    public string Secret { get; init; } = null!;

    public string Issuer { get; init; } = "BookHub";

    public string Audience { get; init; } = "BookHubClient";
}
