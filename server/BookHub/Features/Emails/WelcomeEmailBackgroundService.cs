namespace BookHub.Features.Emails;

public class WelcomeEmailBackgroundService(
    IWelcomeEmailQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<WelcomeEmailBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var emailSender = scope
                    .ServiceProvider
                    .GetRequiredService<IEmailSender>();

                await emailSender.SendWelcome(
                    message.Email,
                    message.Username,
                    message.BaseUrl,
                    stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Failed to send welcome email. UserId={UserId}",
                    message.UserId);
            }
        }
    }
}
