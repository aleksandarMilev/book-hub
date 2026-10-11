namespace BookHub.Tests.Shared.Email;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

// One Mailpit SMTP server for the whole test run, started lazily like PostgresServer. The app
// sends through its real MailKit EmailSender; the tests read what arrived through Mailpit's HTTP
// API (Mailbox). Every factory uses its own mail domain, so parallel classes never see each
// other's messages. Testcontainers' Ryuk removes the container at exit.
internal static class MailpitServer
{
    // Keep this in sync if the image is ever bumped: the API shapes in Mailbox follow v1.x.
    private const string Image = "axllent/mailpit:v1.31.5";

    private const int SmtpPort = 1025;
    private const int ApiPort = 8025;

    // Mailpit answers RCPT TO for any other domain with a real "550 5.1.0 mailbox unavailable",
    // which is how the tests make an email fail without replacing the sender. Test addresses
    // end in .test or .local; UndeliverableDomain doesn't.
    public const string UndeliverableDomain = "undeliverable.invalid";
    private const string AllowedRecipients = @"(?i)\.(test|local)$";

    private static readonly SemaphoreSlim StartLock = new(1, 1);

    // Read outside the lock by the double-checked start in EnsureStarted.
    private static volatile IContainer? container;

    private static HttpClient? api;

    public static string SmtpHost => Started.Hostname;

    public static int SmtpMappedPort => Started.GetMappedPublicPort(SmtpPort);

    // Thread-safe for concurrent requests, and created once with the container.
    public static HttpClient Api => api
        ?? throw new InvalidOperationException($"Call {nameof(EnsureStarted)}() first.");

    private static IContainer Started => container
        ?? throw new InvalidOperationException($"Call {nameof(EnsureStarted)}() first.");

    public static async Task EnsureStarted()
    {
        if (container is not null)
        {
            return;
        }

        await StartLock.WaitAsync();

        try
        {
            if (container is not null)
            {
                return;
            }

            var newContainer = new ContainerBuilder(Image)
                .WithPortBinding(SmtpPort, assignRandomHostPort: true)
                .WithPortBinding(ApiPort, assignRandomHostPort: true)
                .WithEnvironment("MP_SMTP_ALLOWED_RECIPIENTS", AllowedRecipients)
                .WithEnvironment("MP_MAX_MESSAGES", "10000")
                .WithEnvironment("MP_DISABLE_VERSION_CHECK", "true")
                .WithWaitStrategy(Wait
                    .ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(request => request
                        .ForPort(ApiPort)
                        .ForPath("/livez")))
                .Build();

            await newContainer.StartAsync();

            api = new HttpClient
            {
                BaseAddress = new Uri(
                    $"http://{newContainer.Hostname}:{newContainer.GetMappedPublicPort(ApiPort)}/api/v1/"),
            };

            container = newContainer;
        }
        finally
        {
            StartLock.Release();
        }
    }
}
