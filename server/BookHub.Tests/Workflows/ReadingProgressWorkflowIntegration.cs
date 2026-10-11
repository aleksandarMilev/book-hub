namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using Common;
using Features.Books.Service.Models;
using Features.Challenges.Service.Models;
using Features.Challenges.Shared;
using Features.ReadingLists.Shared;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Shared.Seed;
using Shared.Utils;

// A reader sets a yearly goal, reads a book (currently reading, then read), sees the challenge
// progress and the profile counters move, and checks in for the day.
public sealed class ReadingProgressWorkflowIntegration : IAsyncLifetime
{
    private const string ReaderId = "reader-id";

    private readonly WorkflowFactory factory = new();

    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            await data.SeedUser(ReaderId, "reader");
            await data.SeedProfile(ReaderId);

            this.bookId = (await data.SeedBook("A book worth finishing")).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Goal_CurrentlyReading_Read_CheckIn_ShouldMoveTheProgressCountersAndStreak()
    {
        var reader = this.factory.CreateUserClient(ReaderId, "reader");
        var year = DateTime.UtcNow.Year;

        // 1. A yearly goal of two books.
        await (await reader.PutAsJsonAsync(
                "/ReadingChallenges",
                new { year, goalType = ReadingGoalType.Books, goalValue = 2 },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var progress = await this.Progress(reader, year);
        progress.GoalValue.Should().Be(2);
        progress.CurrentValue.Should().Be(0);

        // 2. Start reading the book.
        await (await reader.PostAsJsonAsync(
                "/ReadingLists",
                new { bookId = this.bookId, status = ReadingListStatus.CurrentlyReading },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var lastCurrentlyReading = await (await reader.GetAsync(
                $"/ReadingLists/last-currently-reading?userId={ReaderId}",
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookServiceModel>();

        lastCurrentlyReading.Id.Should().Be(this.bookId);

        var profile = await this.Profile(reader);
        profile.CurrentlyReadingBooksCount.Should().Be(1);
        profile.ReadBooksCount.Should().Be(0);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(0);

        // 3. Finish it: the challenge progress and the counters move.
        await (await reader.PostAsJsonAsync(
                "/ReadingLists",
                new { bookId = this.bookId, status = ReadingListStatus.Read },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(1);

        profile = await this.Profile(reader);
        profile.CurrentlyReadingBooksCount.Should().Be(0);
        profile.ReadBooksCount.Should().Be(1);

        (await this.ListIds(reader, ReadingListStatus.Read)).Should().Equal(this.bookId);
        (await this.ListIds(reader, ReadingListStatus.CurrentlyReading)).Should().BeEmpty();

        // 4. No streak yet; a check-in starts one.
        var streak = await this.Streak(reader);
        streak.CurrentStreak.Should().Be(0);
        streak.CheckedInToday.Should().BeFalse();

        await (await reader.PostAsync("/ReadingChallenges/check-in/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        streak = await this.Streak(reader);
        streak.CurrentStreak.Should().Be(1);
        streak.LongestStreak.Should().Be(1);
        streak.CheckedInToday.Should().BeTrue();

        // 5. One check-in a day.
        await (await reader.PostAsync("/ReadingChallenges/check-in/", null, TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.Conflict);

        (await this.Streak(reader)).CurrentStreak.Should().Be(1);
    }

    private async Task<ReadingChallengeProgressServiceModel> Progress(
        HttpClient reader,
        int year)
        => await (await reader.GetAsync($"/ReadingChallenges/{year}/progress/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReadingChallengeProgressServiceModel>();

    private async Task<ReadingStreakServiceModel> Streak(HttpClient reader)
        => await (await reader.GetAsync("/ReadingChallenges/streak/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReadingStreakServiceModel>();

    private async Task<ProfileServiceModel> Profile(HttpClient reader)
        => await (await reader.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

    private async Task<List<Guid>> ListIds(
        HttpClient reader,
        ReadingListStatus status)
    {
        var page = await (await reader.GetAsync(
                $"/ReadingLists?userId={ReaderId}&status={status}",
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<BookServiceModel>>();

        return page.Items.Select(b => b.Id).ToList();
    }
}
