namespace BookHub.Tests.Challenges;

using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

public sealed class ReadingChallengesIntegration : IAsyncLifetime
{
    private const string UserId = "test-user";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();
        await this.httpClientFactory.WithData(data => data.SeedUser(UserId, "user"));
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Theory]
    [InlineData("/ReadingChallenges/1999/")]
    [InlineData("/ReadingChallenges/2101/progress/")]
    public async Task Get_And_Progress_ShouldReturnBadRequestProblem_WhenTheYearIsOutOfRange(string url)
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var response = await httpClient.GetAsync(url);

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "Invalid year.");
    }

    [Fact]
    public async Task CheckIn_ShouldReturnConflictProblem_AndAlso_ShouldKeepOneCheckIn_WhenAlreadyCheckedInToday()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(UserId);

        var first = await httpClient.PostAsync("/ReadingChallenges/check-in/", content: null);
        var second = await httpClient.PostAsync("/ReadingChallenges/check-in/", content: null);

        first.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await second.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "You already checked in for this date.");

        var checkIns = await this.httpClientFactory.WithData(data => data
            .ReadingCheckIns
            .CountAsync(c => c.UserId == UserId));

        checkIns.Should().Be(1);
    }
}
