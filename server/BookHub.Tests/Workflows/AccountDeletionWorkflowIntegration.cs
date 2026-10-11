namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using Common;
using Features.Books.Service.Models;
using Features.ReadingLists.Shared;
using Features.Reviews.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

// A user deletes their account (a soft delete, with the real JWT pipeline). This pins the
// current behavior: the account and its private data are gone, while the public content it
// wrote stays, and the username and email stay taken (open question 10 in the review).
public sealed class AccountDeletionWorkflowIntegration : IAsyncLifetime
{
    private const string Username = "leaver";

    private readonly BookHubWebApplicationFactory factory = new();

    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        this.bookId = await this.factory.WithData(async data => (await data.SeedBook("A book that outlives a reader")).Id);
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task DeletedAccount_ShouldLoseAccess_AndKeepItsPublicReview_AndKeepItsUsernameTaken()
    {
        var leaver = await this.factory.RegisterAndAuthenticate(Username);
        var stayer = await this.factory.RegisterAndAuthenticate("stayer");

        var leaverId = (await (await leaver.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>()).Id;

        // 1. The user leaves traces: a review, a reading-list entry and a book awaiting approval.
        var review = await (await leaver.PostAsJsonAsync(
                "/Reviews",
                new { content = "Read it twice, would read again.", rating = 5, bookId = this.bookId },
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<ReviewServiceModel>(HttpStatusCode.Created);

        await (await leaver.PostAsJsonAsync(
                "/ReadingLists",
                new { bookId = this.bookId, status = ReadingListStatus.ToRead },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var pendingBook = await (await leaver.PostAsync("/Books", Forms.Book("Left behind"), TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        // 2. The account is deleted.
        await (await leaver.DeleteAsync("/Profile", TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 3. The same signed, unexpired token is rejected, and the password no longer logs in.
        await (await leaver.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.Unauthorized);

        var login = await this.factory.CreateClient().PostAsJsonAsync(
            "/Identity/login/",
            new { credentials = Username, password = BookHubWebApplicationFactory.DefaultPassword, rememberMe = false },
            TestContext.Current.CancellationToken);

        await login.ShouldBeProblem(HttpStatusCode.BadRequest, "Invalid log in attempt!");

        // 4. Other users can't see the profile or its reading lists any more.
        await (await stayer.GetAsync($"/Profile/{leaverId}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        await (await stayer.GetAsync($"/ReadingLists?userId={leaverId}&status={ReadingListStatus.ToRead}", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        // 5. Current behavior: the review stays public under the old username, and it still
        //    counts in the book's rating.
        var reviews = await (await this.factory.CreateClient().GetAsync($"/Reviews/book/{this.bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<ReviewServiceModel>>();

        reviews.Items.Should().ContainSingle(r => r.Id == review.Id && r.CreatedBy == Username);

        var book = await (await this.factory.CreateClient().GetAsync($"/Books/{this.bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>();

        book.RatingsCount.Should().Be(1);
        book.AverageRating.Should().Be(5);

        // 6. Current behavior: the book awaiting approval is kept (unapproved, not deleted).
        var (isApproved, isDeleted) = await this.factory.WithData(async data =>
        {
            var stored = await data
                .Books
                .IgnoreQueryFilters()
                .SingleAsync(b => b.Id == pendingBook.Id, TestContext.Current.CancellationToken);

            return (stored.IsApproved, stored.IsDeleted);
        });

        isApproved.Should().BeFalse();
        isDeleted.Should().BeFalse();

        // 7. Current behavior: the username and email stay taken, because the user is only
        //    soft-deleted.
        var reRegister = await this.factory.CreateClient().PostAsync(
            "/Identity/register/",
            BookHubWebApplicationFactory.RegisterForm(Username),
            TestContext.Current.CancellationToken);

        await reRegister.ShouldBeProblem(
            HttpStatusCode.Conflict,
            $"Username '{Username}' is already taken.");
    }
}
