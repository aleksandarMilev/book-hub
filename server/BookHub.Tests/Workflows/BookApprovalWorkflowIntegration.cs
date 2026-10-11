namespace BookHub.Tests.Workflows;

using System.Net;
using Common;
using Features.Books.Service.Models;
using Features.Notifications.Service.Models;
using Features.Notifications.Shared;
using Features.Search.Service.Models;
using Features.UserProfile.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;
using Shared.Utils;

// A non-admin's book from creation to approval or rejection, through the API, with the app's
// real AdminService (two admins in the Administrator role) and real notifications.
public sealed class BookApprovalWorkflowIntegration : IAsyncLifetime
{
    private const string CreatorId = "creator-id";
    private const string CreatorName = "creator";
    private const string OtherUserId = "other-id";

    private static readonly string[] AdminIds = ["admin-1", "admin-2"];

    private readonly BookHubWebApplicationFactory factory = new();

    private Guid genreId;
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
            await data.SeedUser(OtherUserId, "other");

            this.genreId = (await data.SeedGenre("Workflow genre")).Id;
            this.authorId = (await data.SeedAuthor("Workflow Author")).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Lifecycle_NonAdminCreates_AdminsAreNotified_AdminApproves_CreatorIsNotifiedAndCounted_BookBecomesPublic()
    {
        const string Title = "Approvalworkflow Chronicles";

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var anonymous = this.factory.CreateAnonymousClient();

        // 1. A non-admin creates a book: it is saved unapproved.
        var created = await (await creator.PostAsync(
                "/Books",
                Forms.Book(Title, this.authorId, this.genreId),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        created.IsApproved.Should().BeFalse();
        var bookId = created.Id;

        // 2. Every admin gets a notification.
        foreach (var adminId in AdminIds)
        {
            var notifications = await this.NotificationsOf(await this.factory.CreateAdminClient(adminId, adminId));

            notifications.Should().ContainSingle(n => n.ResourceId == bookId);
            notifications.Single(n => n.ResourceId == bookId).Should().BeEquivalentTo(new
            {
                Message = $"{CreatorName} has created '{Title}'",
                ResourceType = ResourceType.Book,
                IsRead = false,
            });
        }

        // 3. Invisible to anonymous visitors and other users, visible to its creator.
        await (await anonymous.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        await (await (await this.factory.CreateUserClient(OtherUserId, "other")).GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        (await this.PublicListingIds(anonymous, "Approvalworkflow")).Should().NotContain(bookId);

        var creatorView = await (await creator.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>();

        creatorView.Title.Should().Be(Title);

        // 4. An admin approves it.
        var admin = await this.factory.CreateAdminClient(AdminIds[0], AdminIds[0]);

        await (await admin.PatchAsync($"/Administrator/Books/{bookId}/approve/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 5. The creator is notified, and the book counts on their profile.
        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == bookId && n.Message == $"'{Title}' has been approved");

        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedBooksCount.Should().Be(1);

        // 6. Anonymous visitors now see it in the details, the lists and the search.
        var publicView = await (await anonymous.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>();

        publicView.IsApproved.Should().BeTrue();
        publicView.ReadingStatus.Should().BeNull();
        publicView.Author!.Id.Should().Be(this.authorId);
        publicView.Genres.Select(g => g.Id).Should().Equal(this.genreId);

        (await this.PublicListingIds(anonymous, "Approvalworkflow"))
            .Should()
            .Equal(bookId, bookId, bookId);
    }

    [Fact]
    public async Task Rejection_AdminRejects_CreatorIsNotified_BookIsGoneForEveryone()
    {
        const string Title = "Rejectionworkflow Notes";

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);
        var admin = await this.factory.CreateAdminClient(AdminIds[1], AdminIds[1]);
        var anonymous = this.factory.CreateAnonymousClient();

        var created = await (await creator.PostAsync(
                "/Books",
                Forms.Book(Title, this.authorId, this.genreId),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        var bookId = created.Id;

        await (await admin.PatchAsync($"/Administrator/Books/{bookId}/reject/", null, TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        (await this.NotificationsOf(creator))
            .Should()
            .ContainSingle(n => n.ResourceId == bookId && n.Message == $"'{Title}' has been rejected");

        // Gone for everyone, its creator and the admins included.
        HttpClient[] everyone =
        [
            creator,
            anonymous,
            await this.factory.CreateUserClient(OtherUserId, "other"),
        ];

        foreach (var client in everyone)
        {
            await (await client.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken))
                .ShouldBeProblem(HttpStatusCode.NotFound);
        }

        await (await admin.GetAsync($"/Administrator/Books/{bookId}/", TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        (await this.PublicListingIds(anonymous, "Rejectionworkflow")).Should().NotContain(bookId);

        var isDeleted = await this.factory.WithData(data => data
            .Books
            .IgnoreQueryFilters()
            .Where(b => b.Id == bookId)
            .Select(b => b.IsDeleted)
            .SingleAsync(TestContext.Current.CancellationToken));

        isDeleted.Should().BeTrue();

        var profile = await (await creator.GetAsync("/Profile/mine/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<ProfileServiceModel>();

        profile.CreatedBooksCount.Should().Be(0);
    }

    [Fact]
    public async Task Create_ShouldSucceedWithoutNotifications_WhenThereAreNoAdmins()
    {
        await this.factory.WithData(data => data
            .UserRoles
            .ExecuteDeleteAsync(TestContext.Current.CancellationToken));

        var creator = await this.factory.CreateUserClient(CreatorId, CreatorName);

        var created = await (await creator.PostAsync(
                "/Books",
                Forms.Book("A book nobody is told about"),
                TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookDetailsServiceModel>(HttpStatusCode.Created);

        var (bookExists, anyNotification) = await this.factory.WithData(async data => (
            await data.Books.IgnoreQueryFilters().AnyAsync(b => b.Id == created.Id, TestContext.Current.CancellationToken),
            await data.Notifications.AnyAsync(TestContext.Current.CancellationToken)));

        bookExists.Should().BeTrue();
        anyNotification.Should().BeFalse();
    }

    private async Task<List<NotificationServiceModel>> NotificationsOf(HttpClient client)
    {
        var page = await (await client.GetAsync("/Notifications/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<NotificationServiceModel>>();

        return page.Items.ToList();
    }

    // The book IDs in the by-genre list, the by-author list and the search results, in that order.
    private async Task<List<Guid>> PublicListingIds(
        HttpClient client,
        string searchTerm)
    {
        var byGenre = await (await client.GetAsync($"/Books/genre/{this.genreId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<BookServiceModel>>();

        var byAuthor = await (await client.GetAsync($"/Books/author/{this.authorId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<BookServiceModel>>();

        var search = await (await client.GetAsync($"/Search/books/?searchTerm={searchTerm}", TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<SearchBookServiceModel>>();

        return
        [
            .. byGenre.Items.Select(b => b.Id),
            .. byAuthor.Items.Select(b => b.Id),
            .. search.Items.Select(b => b.Id),
        ];
    }
}
