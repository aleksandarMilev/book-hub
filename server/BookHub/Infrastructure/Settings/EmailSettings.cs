namespace BookHub.Infrastructure.Settings;

public class EmailSettings
{
    public string Host { get; init; } = default!;

    public int Port { get; init; }

    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string From { get; init; } = default!;

    public bool UseSsl { get; init; }
}

