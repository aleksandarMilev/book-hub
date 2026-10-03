namespace BookHub.Tests.ReadingLists;

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Data;
using Features.Books.Data.Models;
using Features.Identity.Data.Models;
using Features.ReadingLists.Data.Models;
using Features.ReadingLists.Shared;
using Features.UserProfile.Data.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils;

public sealed class ReadingListsIntegration : IAsyncLifetime
{
    private const string OwnerId = "owner-id";
    private const string OtherUserId = "test-user";
    private const string AdminId = "test-admin-id";

    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    private Guid bookId;

    public async ValueTask InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();

        await this.SeedUser(OwnerId, "owner");
        await this.SeedUser(OtherUserId, "user");
        await this.SeedUser(AdminId, "admin");

        this.bookId = await this.SeedCurrentlyReadingBook(OwnerId);
    }

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
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

    [Fact]
    public async Task All_ShouldReturnNotFoundProblem_WithAGenericDetail_WhenProfileIsPrivate_AndCallerIsAnotherUser()
    {
        await this.SeedProfile(OwnerId, isPrivate: true);

        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId, "user");

        var response = await httpClient.GetAsync(AllUrl(OwnerId));

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The reading list was not found.");
    }

    [Fact]
    public async Task All_ShouldReturnValidationProblem_WhenTheStatusIsInvalid()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OwnerId, "owner");

        var response = await httpClient.GetAsync($"/ReadingLists?userId={OwnerId}&status=99");

        // Model binding rejects undefined enum values before the service runs.
        await response.ShouldBeProblem(HttpStatusCode.BadRequest);

        (await response.Content.ReadAsStringAsync()).Should().Contain("status");
    }

    [Fact]
    public async Task Add_ShouldReturnConflictProblem_WhenTheBookIsAlreadyInTheListWithTheSameStatus()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OwnerId, "owner");

        var response = await httpClient.PostAsJsonAsync(
            "/ReadingLists",
            new { bookId = this.bookId, status = ReadingListStatus.CurrentlyReading });

        await response.ShouldBeProblem(
            HttpStatusCode.Conflict,
            "This book is already in the list with this status.");
    }

    [Fact]
    public async Task Add_ShouldReturnBadRequestProblem_WhenTheBookDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OwnerId, "owner");

        var response = await httpClient.PostAsJsonAsync(
            "/ReadingLists",
            new { bookId = Guid.NewGuid(), status = ReadingListStatus.ToRead });

        await response.ShouldBeProblem(
            HttpStatusCode.BadRequest,
            "The book does not exist.");
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundProblem_WhenTheBookIsNotInTheCallersList()
    {
        var httpClient = this.httpClientFactory.CreateUserClient(OtherUserId, "user");

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/ReadingLists")
        {
            Content = JsonContent.Create(new
            {
                bookId = this.bookId,
                status = ReadingListStatus.CurrentlyReading,
            }),
        };

        var response = await httpClient.SendAsync(request);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The book is not in your reading list.");
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
