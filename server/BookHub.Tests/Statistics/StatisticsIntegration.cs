namespace BookHub.Tests.Statistics;

using System.Net;
using System.Net.Http.Json;
using Data;
using Features.Articles.Data.Models;
using Features.Reviews.Data.Models;
using Features.Statistics.Service.Models;
using FluentAssertions;
using Shared.Seed;

// B-08: the home-page counts must only include rows a visitor can actually see.
public sealed class StatisticsIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory factory = new();

    public async Task InitializeAsync()
        => await this.factory.ResetDatabase();

    public async Task DisposeAsync()
        => await this.factory.DisposeAsync();

    [Fact]
    public async Task All_ShouldCountOnlyVisibleRows_AndAlso_ShouldExcludeSoftDeletedAndUnapproved()
    {
        await this.factory.WithData(SeedVisibleAndHiddenRows);

        var response = await this.factory
            .CreateAnonymousClient()
            .GetAsync("/Statistics");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var statistics = await response.Content.ReadFromJsonAsync<StatisticsServiceModel>();
        AssertVisibleCounts(statistics!);
    }

    [Fact]
    public async Task All_ShouldNotCountUnapprovedRows_WhenAnAdminAsksFirst()
    {
        // The result is cached for every caller, so an admin's request (whose query
        // filters would include unapproved rows) must produce the same counts.
        await this.factory.WithData(SeedVisibleAndHiddenRows);

        var admin = await this.factory
            .CreateAdminClient()
            .GetFromJsonAsync<StatisticsServiceModel>("/Statistics");

        var anonymous = await this.factory
            .CreateAnonymousClient()
            .GetFromJsonAsync<StatisticsServiceModel>("/Statistics");

        AssertVisibleCounts(admin!);
        AssertVisibleCounts(anonymous!);
    }

    private static void AssertVisibleCounts(StatisticsServiceModel statistics)
    {
        statistics.Books.Should().Be(1);
        statistics.Authors.Should().Be(1);
        statistics.Profiles.Should().Be(1);
        statistics.Reviews.Should().Be(1);
        statistics.Articles.Should().Be(1);
        statistics.Genres.Should().Be(2, "the migration seeds \"Other\", plus one visible genre");
    }

    private static async Task SeedVisibleAndHiddenRows(BookHubDbContext data)
    {
        const string ReviewerId = "reviewer";

        await data.SeedProfile(ReviewerId, "Visible", "Profile");
        var deletedProfile = await data.SeedProfile("deleted-profile", "Deleted", "Profile");

        var book = await data.SeedBook("Visible book");
        await data.SeedBook("Unapproved book", isApproved: false);
        var deletedBook = await data.SeedBook("Deleted book");

        await data.SeedAuthor("Visible author");
        await data.SeedAuthor("Unapproved author", isApproved: false);
        var deletedAuthor = await data.SeedAuthor("Deleted author");

        await data.SeedGenre("Visible genre");
        var deletedGenre = await data.SeedGenre("Deleted genre");

        var review = NewReview(book.Id, ReviewerId);
        var deletedReview = NewReview(book.Id, ReviewerId);

        var article = NewArticle("Visible article");
        var deletedArticle = NewArticle("Deleted article");

        data.AddRange(review, deletedReview, article, deletedArticle);
        await data.SaveChangesAsync();

        // Soft deletes (BookHubDbContext turns Remove into IsDeleted = true).
        data.RemoveRange(
            deletedProfile,
            deletedBook,
            deletedAuthor,
            deletedGenre,
            deletedReview,
            deletedArticle);

        await data.SaveChangesAsync();
    }

    private static ReviewDbModel NewReview(Guid bookId, string creatorId)
        => new()
        {
            Id = Guid.NewGuid(),
            Content = new string('r', 100),
            Rating = 5,
            BookId = bookId,
            CreatorId = creatorId,
        };

    private static ArticleDbModel NewArticle(string title)
        => new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Introduction = "An introduction",
            Content = new string('c', 200),
            ImagePath = "/images/articles/test.jpg",
        };
}
