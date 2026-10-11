namespace BookHub.Tests.Shared.Email;

using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

// The messages Mailpit received for one factory's mail domain. Addresses built with Address()
// are unique to the factory (one per test), so a parallel test never reads another's mail.
public sealed partial class Mailbox(string domain)
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(100);

    public string Domain { get; } = domain;

    public string Address(string localPart)
        => $"{localPart}@{this.Domain}";

    public async Task<IReadOnlyList<MessageSummary>> Messages(string to)
    {
        var query = Uri.EscapeDataString($"to:\"{to}\"");
        var result = await MailpitServer.Api.GetFromJsonAsync<SearchResult>(
            $"search?query={query}",
            TestContext.Current.CancellationToken);

        return result!.Messages;
    }

    // Waits for a message to `to` whose subject contains `subject`, and returns it in full.
    public async Task<Message> WaitForMessage(
        string to,
        string subject,
        TimeSpan? timeout = null)
    {
        using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(
            TestContext.Current.CancellationToken);

        timeoutSource.CancelAfter(timeout ?? DefaultTimeout);

        try
        {
            while (true)
            {
                var summary = (await this.Messages(to))
                    .FirstOrDefault(m => m.Subject.Contains(subject, StringComparison.Ordinal));

                if (summary is not null)
                {
                    return (await MailpitServer.Api.GetFromJsonAsync<Message>(
                        $"message/{summary.Id}",
                        TestContext.Current.CancellationToken))!;
                }

                await Task.Delay(PollInterval, timeoutSource.Token);
            }
        }
        catch (OperationCanceledException) when (!TestContext.Current.CancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException($"No email \"{subject}\" to {to} arrived in time.");
        }
    }

    public sealed record SearchResult(IReadOnlyList<MessageSummary> Messages);

    public sealed record MessageSummary(string Id, string Subject, IReadOnlyList<Recipient> To);

    public sealed record Recipient(string Address);

    public sealed record Message(
        string Id,
        string Subject,
        Recipient From,
        IReadOnlyList<Recipient> To,
        string Html)
    {
        // The href targets in the HTML body, HTML-decoded (an emailed "&amp;" is a "&" in the link).
        public IReadOnlyList<string> Links
            => [.. HrefPattern()
                .Matches(this.Html)
                .Select(m => WebUtility.HtmlDecode(m.Groups["url"].Value))];
    }

    [GeneratedRegex("href=\"(?<url>[^\"]+)\"", RegexOptions.IgnoreCase)]
    private static partial Regex HrefPattern();
}
