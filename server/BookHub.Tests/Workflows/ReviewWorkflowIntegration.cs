namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using Features.Authors.Service.Models;
using Features.Books.Service.Models;
using Features.Reviews.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Shared.Seed;
using Shared.Utils;

// Reviews keep the book's and the author's rating and the reviewer's profile counter in step,
// through create, edit and delete; votes on a review toggle.
public sealed class ReviewWorkflowIntegration : IAsyncLifetime
{
    private const string FirstReviewerId = "first-reviewer";
    private const string SecondReviewerId = "second-reviewer";

    private readonly WorkflowFactory factory = new();

    private Guid authorId;
    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            await data.SeedUser(FirstReviewerId, "first");
            await data.SeedProfile(FirstReviewerId);
            await data.SeedUser(SecondReviewerId, "second");
            await data.SeedProfile(SecondReviewerId);

            this.authorId = (await data.SeedAuthor("Reviewed Author")).Id;
            this.bookId = (await data.SeedBook("Reviewed book", authorId: this.authorId)).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task CreateEditDelete_ShouldKeepTheRatingsAndTheReviewCountsInStep()
    {
        var first = this.factory.CreateUserClient(FirstReviewerId, "first");
        var second = this.factory.CreateUserClient(SecondReviewerId, "second");

        // 1. The first review sets the book's and the author's rating.
        var review = await (await first.PostAsJsonAsync(
                "/Reviews",
                this.ReviewBody(rating: 4),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReviewServiceModel>(HttpStatusCode.Created);

        var book = await this.BookDetails();
        book.AverageRating.Should().Be(4);
        book.RatingsCount.Should().Be(1);
        book.Reviews.Select(r => r.Id).Should().Equal(review.Id);

        (await this.AuthorRating()).Should().Be(4);
        (await ReviewsCountOf(first)).Should().Be(1);

        // 2. Editing the rating recomputes the averages without adding a rating.
        await (await first.PutAsJsonAsync(
                $"/Reviews/{review.Id}/",
                this.ReviewBody(rating: 2),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        book = await this.BookDetails();
        book.AverageRating.Should().Be(2);
        book.RatingsCount.Should().Be(1);
        (await this.AuthorRating()).Should().Be(2);
        (await ReviewsCountOf(first)).Should().Be(1);

        // 3. A second reviewer adds a rating.
        await (await second.PostAsJsonAsync(
                "/Reviews",
                this.ReviewBody(rating: 5),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.Created);

        book = await this.BookDetails();
        book.AverageRating.Should().Be(3.5);
        book.RatingsCount.Should().Be(2);
        (await this.AuthorRating()).Should().Be(3.5);

        // 4. Deleting the first review takes its rating out and decrements its author's count
        //    (the review-count part of B-09).
        await (await first.DeleteAsync($"/Reviews/{review.Id}/", TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        book = await this.BookDetails();
        book.AverageRating.Should().Be(5);
        book.RatingsCount.Should().Be(1);
        book.Reviews.Select(r => r.Id).Should().NotContain(review.Id);
        (await this.AuthorRating()).Should().Be(5);
        (await ReviewsCountOf(first)).Should().Be(0);
        (await ReviewsCountOf(second)).Should().Be(1);

        // 5. The first reviewer can review the book again.
        await (await first.PostAsJsonAsync(
                "/Reviews",
                this.ReviewBody(rating: 3),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.Created);

        book = await this.BookDetails();
        book.AverageRating.Should().Be(4);
        book.RatingsCount.Should().Be(2);
        (await ReviewsCountOf(first)).Should().Be(1);
    }

    [Fact]
    public async Task Votes_ShouldToggle_WhenAnotherUserVotesTheSameWayTwice_OrSwitches()
    {
        var review = await this.factory.WithData(data => data.SeedReview(this.bookId, FirstReviewerId));
        var voter = this.factory.CreateUserClient(SecondReviewerId, "second");

        (await this.Vote(voter, review.Id, isUpvote: true)).Should().Be((1, 0));

        // The same vote again takes it back.
        (await this.Vote(voter, review.Id, isUpvote: true)).Should().Be((0, 0));

        (await this.Vote(voter, review.Id, isUpvote: false)).Should().Be((0, 1));

        // The opposite vote switches it.
        (await this.Vote(voter, review.Id, isUpvote: true)).Should().Be((1, 0));
    }

    private object ReviewBody(int rating)
        => new
        {
            content = $"A review that rates the book {rating}.",
            rating,
            bookId = this.bookId,
        };

    // Votes, then returns the review's (upvotes, downvotes) as anyone reads them.
    private async Task<(int Upvotes, int Downvotes)> Vote(
        HttpClient voter,
        Guid reviewId,
        bool isUpvote)
    {
        var votedReviewId = await (await voter.PostAsJsonAsync(
                "/Votes",
                new { reviewId, isUpvote },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<Guid>();

        votedReviewId.Should().Be(reviewId);

        var review = await (await this.factory.CreateAnonymousClient().GetAsync($"/Reviews/{reviewId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReviewServiceModel>();

        return (review.Upvotes, review.Downvotes);
    }

    private async Task<BookDetailsServiceModel> BookDetails()
        => await (await this.factory.CreateAnonymousClient().GetAsync($"/Books/{this.bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>();

    private async Task<double> AuthorRating()
    {
        var author = await (await this.factory.CreateAnonymousClient().GetAsync($"/Authors/{this.authorId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<AuthorDetailsServiceModel>();

        return author.AverageRating;
    }

    private static async Task<int> ReviewsCountOf(HttpClient reviewer)
    {
        var profile = await (await reviewer.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        return profile.ReviewsCount;
    }
}
