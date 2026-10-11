namespace BookHub.Tests.Workflows;

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Common;
using Features.Books.Service.Models;
using Features.ReadingLists.Shared;
using FluentAssertions;
using Shared.Seed;
using Shared.Utils;

// A private profile hides its reading lists from other users (S-04), but not from its owner or
// an admin, until the owner makes the profile public.
public sealed class ReadingListPrivacyWorkflowIntegration : IAsyncLifetime
{
    private const string OwnerId = "owner-id";
    private const string OtherUserId = "other-id";
    private const string AdminId = "admin-id";

    private readonly WorkflowFactory factory = new();

    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();

        await this.factory.WithData(async data =>
        {
            await data.SeedUser(OwnerId, "owner");
            await data.SeedProfile(OwnerId, isPrivate: true);
            await data.SeedUser(OtherUserId, "other");
            await data.SeedProfile(OtherUserId);
            await data.SeedAdmin(AdminId, "admin");

            this.bookId = (await data.SeedBook("A private read")).Id;
        });
    }

    public ValueTask DisposeAsync()
    {
        this.factory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task PrivateProfile_HidesTheReadingListsFromOthers_UntilTheOwnerMakesItPublic()
    {
        var owner = this.factory.CreateUserClient(OwnerId, "owner");
        var other = this.factory.CreateUserClient(OtherUserId, "other");
        var admin = this.factory.CreateAdminClient(AdminId, "admin");

        await (await owner.PostAsJsonAsync(
                "/ReadingLists",
                new { bookId = this.bookId, status = ReadingListStatus.CurrentlyReading },
                TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 1. Private: another user gets 404s and only the private profile model.
        await (await other.GetAsync(ListUrl, TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound, "The reading list was not found.");

        await (await other.GetAsync(LastCurrentlyReadingUrl, TestContext.Current.CancellationToken))
            .ShouldBeProblem(HttpStatusCode.NotFound);

        var privateProfile = await this.ProfileAsSeenBy(other);
        privateProfile.GetProperty("isPrivate").GetBoolean().Should().BeTrue();
        privateProfile.TryGetProperty("readBooksCount", out _).Should().BeFalse();

        // 2. The owner and an admin still see the list.
        foreach (var client in new[] { owner, admin })
        {
            await this.ShouldSeeTheBook(client);
        }

        // 3. The owner makes the profile public.
        await (await owner.PutAsync("/Profile", Forms.Profile(isPrivate: false), TestContext.Current.CancellationToken))
            .ShouldHaveStatus(HttpStatusCode.NoContent);

        // 4. Now the other user sees the list and the full profile.
        await this.ShouldSeeTheBook(other);

        var publicProfile = await this.ProfileAsSeenBy(other);
        publicProfile.GetProperty("isPrivate").GetBoolean().Should().BeFalse();
        publicProfile.GetProperty("currentlyReadingBooksCount").GetInt32().Should().Be(1);
    }

    private static string ListUrl
        => $"/ReadingLists?userId={OwnerId}&status={ReadingListStatus.CurrentlyReading}";

    private static string LastCurrentlyReadingUrl
        => $"/ReadingLists/last-currently-reading?userId={OwnerId}";

    private async Task ShouldSeeTheBook(HttpClient client)
    {
        var list = await (await client.GetAsync(ListUrl, TestContext.Current.CancellationToken))
            .ShouldHaveBody<PaginatedModel<BookServiceModel>>();

        list.Items.Select(b => b.Id).Should().Equal(this.bookId);

        var last = await (await client.GetAsync(LastCurrentlyReadingUrl, TestContext.Current.CancellationToken))
            .ShouldHaveBody<BookServiceModel>();

        last.Id.Should().Be(this.bookId);
    }

    // The JSON as returned: a private profile is a smaller model, not the full one with blanks.
    private async Task<JsonElement> ProfileAsSeenBy(HttpClient client)
        => await (await client.GetAsync($"/Profile/{OwnerId}/", TestContext.Current.CancellationToken))
            .ShouldHaveBody<JsonElement>();
}
