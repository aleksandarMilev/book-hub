namespace BookHub.Features.Emails;

using Infrastructure.Services.ServiceLifetimes;

public interface IWelcomeEmailQueue : ISingletonService
{
    /// <summary>
    /// Queues the message without blocking. Returns false when the queue is full.
    /// </summary>
    bool TryEnqueue(WelcomeEmailMessage message);

    IAsyncEnumerable<WelcomeEmailMessage> ReadAllAsync(
        CancellationToken cancellationToken = default);
}
