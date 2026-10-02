namespace BookHub.Tests.Emails;

using Features.Emails;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

public sealed class WelcomeEmailBackgroundServiceUnit
{
    [Fact]
    public async Task ShouldKeepSending_WhenOneEmailFails()
    {
        var secondSent = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var emailSender = Substitute.For<IEmailSender>();
        emailSender
            .SendWelcome("first@test.local", Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new InvalidOperationException("SMTP server is down.")));

        emailSender
            .SendWelcome("second@test.local", Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                secondSent.TrySetResult();
                return Task.CompletedTask;
            });

        await using var serviceProvider = new ServiceCollection()
            .AddSingleton(emailSender)
            .BuildServiceProvider();

        var queue = new WelcomeEmailQueue();
        var backgroundService = new WelcomeEmailBackgroundService(
            queue,
            serviceProvider.GetRequiredService<IServiceScopeFactory>(),
            NullLogger<WelcomeEmailBackgroundService>.Instance);

        queue.TryEnqueue(new("user-1", "first@test.local", "first", "http://localhost")).Should().BeTrue();
        queue.TryEnqueue(new("user-2", "second@test.local", "second", "http://localhost")).Should().BeTrue();

        await backgroundService.StartAsync(CancellationToken.None);

        await secondSent.Task.WaitAsync(TimeSpan.FromSeconds(10));

        await backgroundService.StopAsync(CancellationToken.None);

        await emailSender
            .Received(1)
            .SendWelcome("first@test.local", "first", "http://localhost", Arg.Any<CancellationToken>());
    }
}
