namespace BookHub.Tests.Workflows;

using System.Net;
using Common;
using Features.Books.Service.Models;
using Features.Notifications.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

using static Features.Books.Shared.Constants.Paths;

// A non-admin's edit of a book goes to a pending row that an admin approves; an admin's own edit
// applies directly and supersedes whatever is pending.
public sealed class BookEditWorkflowIntegration : IAsyncLifetime
{
    private const string CreatorId = "creator-id";
    private const string CreatorName = "creator";

    private static readonly string[] AdminIds = ["admin-1", "admin-2"];

    private readonly BookHubWebApplicationFactory factory = new();

    private Guid fantasyId;
    private Guid sciFiId;
    private Guid authorId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            foreach (var adminId in AdminIds)
            {
                await data.SeedAdmin(adminId, adminId);
            }

            await data.SeedUser(CreatorId, CreatorName);
            await data.SeedProfile(CreatorId);

            this.fantasyId = (await data.SeedGenre("Fantasy")).Id;
            this.sciFiId = (await data.SeedGenre("Sci-Fi")).Id;
            this.authorId = (await data.SeedAuthor("Edit Workflow Author")).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task PendingEdit_IsApprovedByAnAdmin_ThenASecondPendingEdit_IsSupersededByAnAdminsDirectEdit()
    {
        var book = await this.factory.WithData(data => data.SeedBook(
            "Original title",
            creatorId: CreatorId));

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var admin = await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]);
        var anonymous = this.factory.CreateAnonymousClient();

        // 1. The creator edits: the edit waits in a pending row, the book is unchanged.
        await (await creator.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("Pending title", this.authorId, this.fantasyId),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(anonymous, book.Id)).Title.Should().Be("Original title");
        (await this.Details(creator, book.Id)).Title.Should().Be("Pending title");
        (await this.PendingEditsOf(book.Id)).Should().Be(1);

        foreach (var adminId in AdminIds)
        {
            (await this.NotificationsOf(await this.factory.CreateAdminClient(adminId, adminId)))
                .Should()
                .ContainSingle(n => n.ResourceId == book.Id && n.Message == $"{CreatorName} has edited 'Original title'");
        }

        // 2. An admin approves: the changes apply, and the creator is notified.
        await (await admin.PatchAsync($"/Administrator/Books/{book.Id}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var approved = await this.Details(anonymous, book.Id);

        approved.Title.Should().Be("Pending title");
        approved.Author!.Id.Should().Be(this.authorId);
        approved.Genres.Select(g => g.Id).Should().Equal(this.fantasyId);

        (await this.PendingEditsOf(book.Id)).Should().Be(0);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == book.Id && n.Message == "'Pending title' has been approved");

        // 3. A second pending edit, then an admin edits the book directly.
        await (await creator.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("Second pending title", this.authorId, this.fantasyId),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(creator, book.Id)).Title.Should().Be("Second pending title");

        await (await admin.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("Admin title", this.authorId, this.sciFiId),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 4. The admin's edit is live for everyone, and the superseded pending edit is gone (B-25).
        var afterAdminEdit = await this.Details(anonymous, book.Id);

        afterAdminEdit.Title.Should().Be("Admin title");
        afterAdminEdit.Genres.Select(g => g.Id).Should().Equal(this.sciFiId);

        (await this.Details(creator, book.Id)).Title.Should().Be("Admin title");
        (await this.PendingEditsOf(book.Id)).Should().Be(0);

        // Approving again has nothing pending to apply, so it can't bring the old edit back.
        await (await admin.PatchAsync($"/Administrator/Books/{book.Id}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(anonymous, book.Id)).Title.Should().Be("Admin title");
    }

    [Fact]
    public async Task Edit_ShouldCreateANewPendingEdit_AfterThePreviousOneWasRejected()
    {
        // B-26: rejected (and approved) pending edits are soft-deleted, and the unique index on
        // BookEdits.BookId used to count them, so every later edit of the book got a 409.
        var book = await this.factory.WithData(data => data.SeedBook(
            "A book edited twice",
            creatorId: CreatorId));

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);

        await (await creator.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("First proposal"),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        await (await (await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0])).PatchAsync(
                $"/Administrator/Books/{book.Id}/reject/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // Rejecting a pending edit discards the edit, not the book.
        (await this.Details(creator, book.Id)).Title.Should().Be("A book edited twice");

        await (await creator.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("Second proposal"),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(creator, book.Id)).Title.Should().Be("Second proposal");
        (await this.PendingEditsOf(book.Id)).Should().Be(1);
    }

    [Fact]
    public async Task Approve_ShouldFallBackToOtherGenre_WhenThePendingGenresWereDeleted()
    {
        var book = await this.factory.WithData(data => data.SeedBook(
            "A book losing its genre",
            creatorId: CreatorId));

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);

        await (await creator.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("A book losing its genre", null, this.sciFiId),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // The genre is deleted while the edit waits for approval.
        await this.factory.WithData(async data =>
        {
            var genre = await data.Genres.SingleAsync(g => g.Id == this.sciFiId, TestContext.Current.CancellationToken);

            data.Genres.Remove(genre);
            await data.SaveChangesAsync(TestContext.Current.CancellationToken);
        });

        await (await (await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0])).PatchAsync(
                $"/Administrator/Books/{book.Id}/approve/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var genreIds = await this.factory.WithData(data => data
            .BooksGenres
            .Where(bg => bg.BookId == book.Id)
            .Select(bg => bg.GenreId)
            .ToListAsync(TestContext.Current.CancellationToken));

        genreIds.Should().Equal(TestSeeder.OtherGenreId);
    }

    [Fact]
    public async Task Approve_ShouldApplyTheEdit_AndCountTheBook_WhenTheCreatorEditedItWhilePending()
    {
        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);

        var created = await (await creator.PostAsync(
                "/Books",
                Forms.Book("Written in haste"),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        await (await creator.PutAsync(
                $"/Books/{created.Id}/",
                Forms.Book("Edited before approval"),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        await (await (await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0])).PatchAsync(
                $"/Administrator/Books/{created.Id}/approve/",
                null,
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var publicView = await this.Details(this.factory.CreateAnonymousClient(), created.Id);

        publicView.Title.Should().Be("Edited before approval");
        publicView.IsApproved.Should().BeTrue();

        // B-24: approving through the pending-edit branch still counts the newly approved book.
        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedBooksCount.Should().Be(1);
    }

    [Fact]
    public async Task Covers_ThroughPendingEdits_AnApproval_ARejection_AndAnAdminsEdit_OnlyTheLiveCoverStaysOnDisk()
    {
        var cover = this.factory.SeedImageFile(ImagePathPrefix, "cover.jpg");
        var book = await this.factory.WithData(data => data.SeedBook(
            "A book with a cover",
            creatorId: CreatorId,
            imagePath: cover));

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var admin = await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]);

        // 1. A pending edit with a new cover: the file waits in the pending folder.
        await this.EditWithCover(creator, book.Id, "First proposal");

        var firstPending = await this.PendingImageOf(book.Id);

        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(firstPending);
        this.factory.ImageFiles(ImagePathPrefix).Should().Equal(cover);

        // 2. The creator changes their mind: the second cover replaces the first pending file.
        await this.EditWithCover(creator, book.Id, "Second proposal");

        var secondPending = await this.PendingImageOf(book.Id);

        secondPending.Should().NotBe(firstPending);
        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(secondPending);

        // 3. Approved: the pending cover goes live, and the old cover's file is deleted.
        await (await admin.PatchAsync($"/Administrator/Books/{book.Id}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.Details(this.factory.CreateAnonymousClient(), book.Id)).ImagePath.Should().Be(secondPending);
        this.factory.ImageFiles(ImagePathPrefix).Should().BeEmpty();
        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(secondPending);

        // 4. Another pending cover, rejected: its file is deleted, the live cover stays.
        await this.EditWithCover(creator, book.Id, "Rejected proposal");

        var rejected = await this.PendingImageOf(book.Id);

        await (await admin.PatchAsync($"/Administrator/Books/{book.Id}/reject/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        this.factory.ImageExists(rejected).Should().BeFalse();
        this.factory.ImageFiles(PendingImagePathPrefix).Should().Equal(secondPending);

        // 5. A pending cover superseded by an admin's own new cover: the pending file and the
        //    previous live cover are both deleted, and only the admin's cover is left.
        await this.EditWithCover(creator, book.Id, "Superseded proposal");

        var superseded = await this.PendingImageOf(book.Id);

        await (await admin.PutAsync(
                $"/Books/{book.Id}/",
                Forms.Book("The admin's version").WithImage(),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        var live = (await this.Details(this.factory.CreateAnonymousClient(), book.Id)).ImagePath;

        this.factory.ImageExists(superseded).Should().BeFalse();
        this.factory.ImageFiles(PendingImagePathPrefix).Should().BeEmpty();
        this.factory.ImageFiles(ImagePathPrefix).Should().Equal(live);
    }

    private async Task EditWithCover(
        HttpClient client,
        Guid bookId,
        string title)
        => await (await client.PutAsync(
                $"/Books/{bookId}/",
                Forms.Book(title).WithImage(),
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

    private Task<string> PendingImageOf(Guid bookId)
        => this.factory.WithData(data => data
            .BookEdits
            .Where(e => e.BookId == bookId)
            .Select(e => e.ImagePath)
            .SingleAsync(TestContext.Current.CancellationToken));

    private async Task<BookDetailsServiceModel> Details(
        HttpClient client,
        Guid bookId)
        => await (await client.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>();

    private async Task<List<NotificationServiceModel>> NotificationsOf(HttpClient client)
    {
        var page = await (await client.GetAsync("/Notifications/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<NotificationServiceModel>>();

        return page.Items.ToList();
    }

    private Task<int> PendingEditsOf(Guid bookId)
        => this.factory.WithData(data => data
            .BookEdits
            .CountAsync(e => e.BookId == bookId, TestContext.Current.CancellationToken));
}
