namespace BookHub.Tests.Shared.Mocks;

using System.Collections.Concurrent;
using Features.Emails;

// Email is a true external boundary. Records every email the app tries to send, so a test can
// follow a link out of it (a password reset), and can make the welcome email fail like an SMTP
// outage. One instance per factory (per host), so parallel classes never share it.
public sealed class RecordingEmailSender : IEmailSender
{
    private readonly ConcurrentQueue<SentEmail> welcomeEmails = new();
    private readonly ConcurrentQueue<SentEmail> passwordResets = new();

    private readonly TaskCompletionSource<string> welcomeAttempted =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public bool FailWelcomeEmails { get; set; }

    // Completes with the address of the first welcome email the app tried to send.
    public Task<string> WelcomeAttempted => this.welcomeAttempted.Task;

    public IReadOnlyCollection<SentEmail> WelcomeEmails => this.welcomeEmails;

    public IReadOnlyCollection<SentEmail> PasswordResets => this.passwordResets;

    public Task SendWelcome(
        string email,
        string username,
        string baseUrl,
        CancellationToken cancellationToken = default)
    {
        this.welcomeAttempted.TrySetResult(email);

        if (this.FailWelcomeEmails)
        {
            throw new InvalidOperationException("SMTP server is down.");
        }

        this.welcomeEmails.Enqueue(new(email, baseUrl));
        return Task.CompletedTask;
    }

    public Task SendPasswordReset(
        string email,
        string resetUrl,
        CancellationToken cancellationToken = default)
    {
        this.passwordResets.Enqueue(new(email, resetUrl));
        return Task.CompletedTask;
    }

    public sealed record SentEmail(string To, string Url);
}
