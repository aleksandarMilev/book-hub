namespace BookHub.Tests.Reviews;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

public sealed class ReviewsIntegration : IAsyncLifetime
{
    private const string AuthorOfReviewId = "review-author";
    private const string OtherUserId = "test-user";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();

        this.bookId = await this.httpClientFactory.WithData(async data =>
        {
            await data.SeedUser(OtherUserId, "user");
            var book = await data.SeedBook("Reviewed book");

            return book.Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Create_ShouldReturnConflictProblem_WhenTheUserAlreadyReviewedTheBook()
    {
        await this.httpClientFactory.WithData(data => data.SeedReview(this.bookId, OtherUserId));
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PostAsJsonAsync(
            "/Reviews",
            ReviewRequest(this.bookId));

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "You have already reviewed this book.");

        var reviewsCount = await this.httpClientFactory.WithData(data => data
            .Reviews
            .CountAsync(r => r.CreatorId == OtherUserId));

        reviewsCount.Should().Be(1);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequestProblem_WhenTheBookDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PostAsJsonAsync(
            "/Reviews",
            ReviewRequest(Guid.NewGuid()));

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "The book does not exist.");
    }

    [Fact]
    public async Task Edit_ShouldReturnForbiddenProblem_AndAlso_ShouldNotChangeTheReview_WhenTheCallerIsNotTheCreator()
    {
        var review = await this.httpClientFactory.WithData(
            data => data.SeedReview(this.bookId, AuthorOfReviewId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PutAsJsonAsync(
            $"/Reviews/{review.Id}/",
            ReviewRequest(this.bookId, content: "Hijacked review content"));

        var problem = await response.ShouldBeProblem(
            HttpStatusCode.Forbidden,
            "You are not allowed to modify this review.");

        // S-11: no IDs or internal type names in the response.
        problem.Detail.Should().NotContain(OtherUserId);
        problem.Detail.Should().NotContain(review.Id.ToString());

        var content = await this.httpClientFactory.WithData(data => data
            .Reviews
            .Where(r => r.Id == review.Id)
            .Select(r => r.Content)
            .SingleAsync());

        content.Should().Be("A seeded review");
    }

    [Fact]
    public async Task Edit_ShouldReturnNotFoundProblem_WhenTheReviewDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PutAsJsonAsync(
            $"/Reviews/{Guid.NewGuid()}/",
            ReviewRequest(this.bookId));

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The review was not found.");
    }

    [Fact]
    public async Task Delete_ShouldReturnForbiddenProblem_AndAlso_ShouldKeepTheReview_WhenTheCallerIsNotTheCreator()
    {
        var review = await this.httpClientFactory.WithData(
            data => data.SeedReview(this.bookId, AuthorOfReviewId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.DeleteAsync($"/Reviews/{review.Id}/");

        await response.ShouldBeProblem(
            HttpStatusCode.Forbidden,
            "You are not allowed to modify this review.");

        var exists = await this.httpClientFactory.WithData(data => data
            .Reviews
            .AnyAsync(r => r.Id == review.Id));

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task Vote_ShouldReturnNotFoundProblem_WhenTheReviewDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PostAsJsonAsync(
            "/Votes",
            new { reviewId = Guid.NewGuid(), isUpvote = true });

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The review was not found.");

        var votesCount = await this.httpClientFactory.WithData(data => data.Votes.CountAsync());
        votesCount.Should().Be(0);
    }

    [Fact]
    public async Task Vote_ShouldReturnOk_WithTheReviewId_WhenTheReviewExists()
    {
        var review = await this.httpClientFactory.WithData(
            data => data.SeedReview(this.bookId, AuthorOfReviewId));

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId);

        var response = await httpClient.PostAsJsonAsync(
            "/Votes",
            new { reviewId = review.Id, isUpvote = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<Guid>()).Should().Be(review.Id);
    }

    private static object ReviewRequest(
        Guid bookId,
        string content = "A perfectly fine review")
        => new { content, rating = 4, bookId };
}
