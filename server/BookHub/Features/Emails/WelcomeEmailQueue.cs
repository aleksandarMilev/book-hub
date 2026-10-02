namespace BookHub.Features.Emails;

using System.Threading.Channels;

// In-memory only: queued emails are lost on restart (see docs/backlog.md).
public class WelcomeEmailQueue : IWelcomeEmailQueue
{
    private const int Capacity = 1_000;

    private readonly Channel<WelcomeEmailMessage> channel =
        Channel.CreateBounded<WelcomeEmailMessage>(
            new BoundedChannelOptions(Capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true
            });

    public bool TryEnqueue(WelcomeEmailMessage message)
        => this.channel.Writer.TryWrite(message);

    public IAsyncEnumerable<WelcomeEmailMessage> ReadAllAsync(
        CancellationToken cancellationToken = default)
        => this.channel.Reader.ReadAllAsync(cancellationToken);
}
