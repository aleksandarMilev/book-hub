namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using Common;
using Features.Books.Service.Models;
using Features.Challenges.Data.Models;
using Features.Challenges.Service.Models;
using Features.Challenges.Shared;
using Features.ReadingLists.Shared;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

// What ReadingProgressWorkflowIntegration doesn't cover: a reader changes their mind. Books move
// out of "read", leave the lists and come back, a goal changes from books to pages, and check-ins
// span several days. The profile counters and the challenge progress must follow every step.
public sealed class ReadingListLifecycleWorkflowIntegration : IAsyncLifetime
{
    private const string ReaderId = "reader-id";

    private readonly BookHubWebApplicationFactory factory = new();

    private Guid longBookId;
    private Guid shortBookId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            await data.SeedProfile(ReaderId);

            var longBook = await data.SeedBook("A long book");
            var shortBook = await data.SeedBook("A short book");

            longBook.Pages = 300;
            shortBook.Pages = 120;
            await data.SaveChangesAsync();

            this.longBookId = longBook.Id;
            this.shortBookId = shortBook.Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Books_MoveOutOfRead_LeaveTheLists_AndComeBack_WithAPagesGoal()
    {
        var reader = await this.factory.CreateUserClient(ReaderId);
        var year = DateTime.UtcNow.Year;

        // 1. A goal of two books, changed to 400 pages: the same challenge is updated.
        await this.SetGoal(reader, year, ReadingGoalType.Books, 2);
        await this.SetGoal(reader, year, ReadingGoalType.Pages, 400);

        var goal = await (await reader.GetAsync($"/ReadingChallenges/{year}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReadingChallengeServiceModel>();

        goal.GoalType.Should().Be(ReadingGoalType.Pages);
        goal.GoalValue.Should().Be(400);

        var challenges = await this.factory.WithData(data => data
            .ReadingChallenges
            .CountAsync(c => c.UserId == ReaderId, TestContext.Current.CancellationToken));

        challenges.Should().Be(1);

        // 2. Both books read: the progress counts their pages.
        await this.Add(reader, this.longBookId, ReadingListStatus.Read);
        await this.Add(reader, this.shortBookId, ReadingListStatus.Read);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(420);
        await this.ShouldHaveCounts(reader, read: 2, toRead: 0, currentlyReading: 0);

        // 3. The long book moves back to "to read": it no longer counts as read this year.
        await this.Add(reader, this.longBookId, ReadingListStatus.ToRead);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(120);
        await this.ShouldHaveCounts(reader, read: 1, toRead: 1, currentlyReading: 0);
        (await this.CompletedOn(this.longBookId)).Should().BeNull();

        // 4. Both books leave the lists.
        await this.Remove(reader, this.shortBookId, ReadingListStatus.Read);
        await this.Remove(reader, this.longBookId, ReadingListStatus.ToRead);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(0);
        await this.ShouldHaveCounts(reader, read: 0, toRead: 0, currentlyReading: 0);
        (await this.ListIds(reader, ReadingListStatus.Read)).Should().BeEmpty();

        // Removing a book that isn't in the lists any more is a 404.
        await (await this.Delete(reader, this.shortBookId, ReadingListStatus.Read))
            .ShouldBeProblem(HttpStatusCode.NotFound, "The book is not in your reading list.");

        // 5. The short book comes back as read: its old row is restored, and it counts once.
        await this.Add(reader, this.shortBookId, ReadingListStatus.Read);

        (await this.Progress(reader, year)).CurrentValue.Should().Be(120);
        await this.ShouldHaveCounts(reader, read: 1, toRead: 0, currentlyReading: 0);
        (await this.ListIds(reader, ReadingListStatus.Read)).Should().Equal(this.shortBookId);

        var rows = await this.factory.WithData(data => data
            .ReadingLists
            .IgnoreQueryFilters()
            .CountAsync(rl => rl.UserId == ReaderId && rl.BookId == this.shortBookId, TestContext.Current.CancellationToken));

        rows.Should().Be(1);
    }

    [Fact]
    public async Task Streak_ShouldCountConsecutiveDays_AndRememberTheLongestRun()
    {
        var reader = await this.factory.CreateUserClient(ReaderId);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Earlier check-ins: a five-day run two weeks ago, and the two days before today.
        await this.factory.WithData(async data =>
        {
            int[] daysAgo = [14, 13, 12, 11, 10, 2, 1];

            foreach (var days in daysAgo)
            {
                data.ReadingCheckIns.Add(new ReadingCheckInDbModel
                {
                    UserId = ReaderId,
                    Date = today.AddDays(-days),
                });
            }

            await data.SaveChangesAsync();
        });

        // Not checked in today yet: the run up to yesterday still counts.
        var streak = await this.Streak(reader);

        streak.CurrentStreak.Should().Be(2);
        streak.LongestStreak.Should().Be(5);
        streak.CheckedInToday.Should().BeFalse();

        await (await reader.PostAsync("/ReadingChallenges/check-in/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        streak = await this.Streak(reader);

        streak.CurrentStreak.Should().Be(3);
        streak.LongestStreak.Should().Be(5);
        streak.CheckedInToday.Should().BeTrue();

        // A missed day breaks the current run, not the longest one.
        await this.factory.WithData(data => data
            .ReadingCheckIns
            .Where(c => c.UserId == ReaderId && c.Date == today.AddDays(-1))
            .ExecuteDeleteAsync(TestContext.Current.CancellationToken));

        streak = await this.Streak(reader);

        streak.CurrentStreak.Should().Be(1);
        streak.LongestStreak.Should().Be(5);
    }

    private async Task SetGoal(
        HttpClient reader,
        int year,
        ReadingGoalType goalType,
        int goalValue)
        => await (await reader.PutAsJsonAsync(
                "/ReadingChallenges",
                new { year, goalType, goalValue },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

    private async Task Add(
        HttpClient reader,
        Guid bookId,
        ReadingListStatus status)
        => await (await reader.PostAsJsonAsync(
                "/ReadingLists",
                new { bookId, status },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

    private async Task Remove(
        HttpClient reader,
        Guid bookId,
        ReadingListStatus status)
        => await (await this.Delete(reader, bookId, status)).ShouldHaveStatus(HttpStatusCode.NoContent);

    private async Task<HttpResponseMessage> Delete(
        HttpClient reader,
        Guid bookId,
        ReadingListStatus status)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, "/ReadingLists")
        {
            Content = JsonContent.Create(new { bookId, status }),
        };

        return await reader.SendAsync(request, TestContext.Current.CancellationToken);
    }

    private async Task ShouldHaveCounts(
        HttpClient reader,
        int read,
        int toRead,
        int currentlyReading)
    {
        var profile = await (await reader.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.ReadBooksCount.Should().Be(read);
        profile.ToReadBooksCount.Should().Be(toRead);
        profile.CurrentlyReadingBooksCount.Should().Be(currentlyReading);
    }

    private Task<DateTime?> CompletedOn(Guid bookId)
        => this.factory.WithData(data => data
            .ReadingLists
            .Where(rl => rl.UserId == ReaderId && rl.BookId == bookId)
            .Select(rl => rl.CompletedOn)
            .SingleAsync(TestContext.Current.CancellationToken));

    private async Task<ReadingChallengeProgressServiceModel> Progress(
        HttpClient reader,
        int year)
        => await (await reader.GetAsync($"/ReadingChallenges/{year}/progress/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReadingChallengeProgressServiceModel>();

    private async Task<ReadingStreakServiceModel> Streak(HttpClient reader)
        => await (await reader.GetAsync("/ReadingChallenges/streak/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReadingStreakServiceModel>();

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
