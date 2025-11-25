namespace BookHub;

public class ApplicationSettings
{
    public ApplicationSettings()
        => this.Secret = default!;
        
    public string Secret { get; init; } = null!;

    public string Issuer { get; init; } = "BookHub";

    public string Audience { get; init; } = "BookHubClient";
}
