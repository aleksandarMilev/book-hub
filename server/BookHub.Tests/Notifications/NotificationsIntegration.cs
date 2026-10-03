namespace BookHub.Tests.Notifications;

using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

// Notifications are private: someone else's notification is reported as not found (404),
// never as forbidden (403), so the response doesn't reveal that it exists.
public sealed class NotificationsIntegration : IAsyncLifetime
{
    private const string ReceiverId = "receiver";
    private const string OtherUserId = "test-user";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();
        await this.httpClientFactory.WithData(data => data.SeedUser(OtherUserId, "user"));
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundProblem_AndAlso_ShouldKeepTheNotification_WhenItBelongsToAnotherUser()
    {
        var notification = await this.httpClientFactory.WithData(
            data => data.SeedNotification(ReceiverId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.DeleteAsync($"/Notifications/{notification.Id}/");

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The notification was not found.");

        var exists = await this.httpClientFactory.WithData(data => data
            .Notifications
            .AnyAsync(n => n.Id == notification.Id));

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldReturnTheSameProblem_ForAMissingNotification_AsForAnotherUsersNotification()
    {
        var notification = await this.httpClientFactory.WithData(
            data => data.SeedNotification(ReceiverId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var othersResponse = await httpClient.DeleteAsync($"/Notifications/{notification.Id}/");
        var missingResponse = await httpClient.DeleteAsync($"/Notifications/{Guid.NewGuid()}/");

        var othersProblem = await othersResponse.ShouldBeProblem(HttpStatusCode.NotFound);
        var missingProblem = await missingResponse.ShouldBeProblem(HttpStatusCode.NotFound);

        othersProblem.Detail.Should().Be(missingProblem.Detail);
    }

    [Fact]
    public async Task MarkRead_ShouldReturnNotFoundProblem_AndAlso_ShouldNotMarkIt_WhenItBelongsToAnotherUser()
    {
        var notification = await this.httpClientFactory.WithData(
            data => data.SeedNotification(ReceiverId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PatchAsync(
            $"/Notifications/{notification.Id}/read/",
            content: null);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The notification was not found.");

        var isRead = await this.httpClientFactory.WithData(data => data
            .Notifications
            .Where(n => n.Id == notification.Id)
            .Select(n => n.IsRead)
            .SingleAsync());

        isRead.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_AndAlso_ShouldDeleteIt_WhenTheCallerIsTheReceiver()
    {
        var notification = await this.httpClientFactory.WithData(
            data => data.SeedNotification(ReceiverId));

        var httpClient = this.httpClientFactory.CreateUserClient(ReceiverId);

        var response = await httpClient.DeleteAsync($"/Notifications/{notification.Id}/");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var exists = await this.httpClientFactory.WithData(data => data
            .Notifications
            .AnyAsync(n => n.Id == notification.Id));

        exists.Should().BeFalse();
    }
}
