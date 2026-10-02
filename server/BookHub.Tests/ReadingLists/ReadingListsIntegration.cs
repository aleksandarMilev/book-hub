namespace BookHub.Tests.ReadingLists;

using System.Net;
using System.Text.Json;
using Data;
using Features.Books.Data.Models;
using Features.Identity.Data.Models;
using Features.ReadingLists.Data.Models;
using Features.ReadingLists.Shared;
using Features.UserProfile.Data.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

public sealed class ReadingListsIntegration : IAsyncLifetime
{
    private const string OwnerId = "owner-id";
    private const string OtherUserId = "test-user";
    private const string AdminId = "test-admin-id";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    private Guid bookId;

    public async Task InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();

        await this.SeedUser(OwnerId, "owner");
        await this.SeedUser(OtherUserId, "user");
        await this.SeedUser(AdminId, "admin");

        this.bookId = await this.SeedCurrentlyReadingBook(OwnerId);
    }

    public Task DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task All_And_LastCurrentlyReading_ShouldReturnNotFound_WhenProfileIsPrivate_AndCallerIsAnotherUser()
    {
        await this.SeedProfile(OwnerId, isPrivate: true);

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId, "user");

        var allResponse = await httpClient.GetAsync(AllUrl(OwnerId));
        var lastResponse = await httpClient.GetAsync(LastCurrentlyReadingUrl(OwnerId));

        allResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        lastResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task All_And_LastCurrentlyReading_ShouldReturnNotFound_WhenProfileDoesNotExist_AndCallerIsAnotherUser()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId, "user");

        var allResponse = await httpClient.GetAsync(AllUrl(OwnerId));
        var lastResponse = await httpClient.GetAsync(LastCurrentlyReadingUrl(OwnerId));

        allResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        lastResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task All_And_LastCurrentlyReading_ShouldReturnTheBook_WhenProfileIsPrivate_AndCallerIsTheOwner()
    {
        await this.SeedProfile(OwnerId, isPrivate: true);

        var httpClient = this.httpClientFactory.CreateUserClient(OwnerId, "owner");

        await this.AssertListsAreVisible(httpClient);
    }

    [Fact]
    public async Task All_And_LastCurrentlyReading_ShouldReturnTheBook_WhenProfileIsPrivate_AndCallerIsAdmin()
    {
        await this.SeedProfile(OwnerId, isPrivate: true);

        var httpClient = this.httpClientFactory.CreateAdminClient(AdminId, "admin");

        await this.AssertListsAreVisible(httpClient);
    }

    [Fact]
    public async Task All_And_LastCurrentlyReading_ShouldReturnTheBook_WhenProfileIsPublic_AndCallerIsAnotherUser()
    {
        await this.SeedProfile(OwnerId, isPrivate: false);

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId, "user");

        await this.AssertListsAreVisible(httpClient);
    }

    private static string AllUrl(string userId)
        => $"/ReadingLists?userId={userId}&status={ReadingListStatus.CurrentlyReading}";

    // Also covers B-05: the route is relative to the controller (no leading slash).
    private static string LastCurrentlyReadingUrl(string userId)
        => $"/ReadingLists/last-currently-reading?userId={userId}";

    private async Task AssertListsAreVisible(HttpClient httpClient)
    {
        var allResponse = await httpClient.GetAsync(AllUrl(OwnerId));
        allResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var allJson = JsonDocument.Parse(await allResponse.Content.ReadAsStringAsync());
        var items = allJson.RootElement.GetProperty("items");

        items.GetArrayLength().Should().Be(1);
        items[0].GetProperty("id").GetGuid().Should().Be(this.bookId);

        var lastResponse = await httpClient.GetAsync(LastCurrentlyReadingUrl(OwnerId));
        lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var lastJson = JsonDocument.Parse(await lastResponse.Content.ReadAsStringAsync());
        lastJson.RootElement.GetProperty("id").GetGuid().Should().Be(this.bookId);
    }

    private async Task SeedUser(string id, string username)
    {
        using var scope = this.httpClientFactory.Services.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();

        var email = $"{username}@test.local";

        data.Users.Add(new UserDbModel
        {
            Id = id,
            UserName = username,
            NormalizedUserName = username.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow
        });

        await data.SaveChangesAsync();
    }

    private async Task SeedProfile(string userId, bool isPrivate)
    {
        using var scope = this.httpClientFactory.Services.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();

        data.Profiles.Add(new UserProfile
        {
            UserId = userId,
            FirstName = "Owner",
            LastName = "Profile",
            ImagePath = "/images/profiles/default.jpg",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsPrivate = isPrivate
        });

        await data.SaveChangesAsync();
    }

    private async Task<Guid> SeedCurrentlyReadingBook(string userId)
    {
        using var scope = this.httpClientFactory.Services.CreateScope();
        var data = scope.ServiceProvider.GetRequiredService<BookHubDbContext>();

        var book = new BookDbModel
        {
            Id = Guid.NewGuid(),
            Title = "Currently reading book",
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            ImagePath = "/images/books/seed.jpg",
            CreatorId = userId,
            IsApproved = true
        };

        data.Books.Add(book);
        data.ReadingLists.Add(new ReadingListDbModel
        {
            UserId = userId,
            BookId = book.Id,
            Status = ReadingListStatus.CurrentlyReading
        });

        await data.SaveChangesAsync();

        return book.Id;
    }
}
