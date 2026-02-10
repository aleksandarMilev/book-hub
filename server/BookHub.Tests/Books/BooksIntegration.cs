namespace BookHub.Tests.Books;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Common;
using Data;
using Data.Models.Shared.BookGenre.Models;
using Features.Authors.Data.Models;
using Features.Authors.Shared;
using Features.Books.Data.Models;
using Features.Books.Service.Models;
using Features.Genres.Data.Models;
using Features.Identity.Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using static Features.Books.Shared.Constants.Paths;
using static Shared.Utils.Constants;

public sealed class BooksIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    private static readonly Guid OtherGenreId = new("52e607d4-c347-440a-8d55-cf2e01d88a6c");

    public async Task InitializeAsync()
    {
        await this.httpClientFactory.ResetDatabase();

        await this.SeedUser("test-user", "user");
        await this.SeedUser("test-admin-id", "admin");

        await this.SeedGenre(OtherGenreId, "Other");
        await this.SeedGenre(
            Guid.Parse("11111111-1111-1111-1111-111111111111"), "Fantasy");

        await this.SeedAuthor(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Seed author");
    }

    public Task DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task TopThree_ShouldReturnThreeBooksOrderedByAverageRatingDesc()
    {
        var book1Id = await this.SeedBook(averageRating: 4.6);
        var book2Id = await this.SeedBook(averageRating: 3.8);
        var book3Id = await this.SeedBook(averageRating: 5.0);

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync("/Books/top/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<BookServiceModel>>();

        body.Should().NotBeNull();

        var list = body.ToList();
        list.Should().HaveCount(3);

        list[0].Id.Should().Be(book3Id);
        list[1].Id.Should().Be(book1Id);
        list[2].Id.Should().Be(book2Id);
    }

    [Fact]
    public async Task ByGenre_ShouldReturnPaginatedBooks_WhenGenreHasBooks()
    {
        var fantasyId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var book1Id = await this.SeedBook(
            averageRating: 4.0,
            genreIds: [fantasyId]);

        var book2Id = await this.SeedBook(
            averageRating: 5.0,
            genreIds: [fantasyId]);

        await this.SeedBook(
            averageRating: 3.0,
            genreIds: [OtherGenreId]);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.GetAsync($"/Books/genre/{fantasyId}?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<PaginatedModel<BookServiceModel>>();

        body.Should().NotBeNull();
        body.TotalItems.Should().Be(2);

        var ids = body.Items.Select(i => i.Id).ToList();
        ids.Should().Contain([book1Id, book2Id]);

        body.Items.First().AverageRating.Should().Be(5.0);
    }

    [Fact]
    public async Task ByGenre_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var fantasyId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(
            $"/Books/genre/{fantasyId}?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ByAuthor_ShouldReturnPaginatedBooks_WhenAuthorHasBooks()
    {
        var authorId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var book1Id = await this.SeedBook(
            averageRating:
            2.0, authorId: authorId);

        var book2Id = await this.SeedBook(
            averageRating: 4.0,
            authorId: authorId);

        var _ = await this.SeedBook(
            averageRating: 5.0,
            authorId: null);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.GetAsync($"/Books/author/{authorId}?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<PaginatedModel<BookServiceModel>>();

        body.Should().NotBeNull();
        body.TotalItems.Should().Be(2);

        var ids = body.Items.Select(i => i.Id).ToList();
        ids.Should().Contain([book1Id, book2Id]);

        body.Items.First().AverageRating.Should().Be(4.0);
    }

    [Fact]
    public async Task ByAuthor_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var authorId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(
            $"/Books/author/{authorId}?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenNoBookWithSuchIdInDb()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.GetAsync($"/Books/{nonExistingId}/");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldPersistTheBookInTheDb_AndAlso_ShouldSetDefaultImage_WhenNoImageProvided()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildBookForm(
            title: "A valid book title long enough",
            shortDescription: "A valid short description",
            longDescription: new string('l', 200),
            authorId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            publishedDate: new DateTime(2000, 1, 1),
            genreIds: []);

        var response = await httpClient.PostAsync("/Books", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdBook = await response
            .Content
            .ReadFromJsonAsync<BookDetailsServiceModel>();

        createdBook.Should().NotBeNull();
        createdBook.Id.Should().NotBeEmpty();
        createdBook.ImagePath.Should().Be(DefaultImagePath);

        response
            .Headers
            .Location
            .ToString()
            .Should()
            .Contain($"/Books/{createdBook.Id}");

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == createdBook.Id);

        dbModel.ImagePath.Should().Be(DefaultImagePath);
        dbModel.IsApproved.Should().BeFalse();

        var mapEntities = await data
            .BooksGenres
            .AsNoTracking()
            .Where(bg => bg.BookId == createdBook.Id)
            .ToListAsync();

        mapEntities.Should().HaveCount(1);
        mapEntities[0].GenreId.Should().Be(OtherGenreId);
    }

    [Fact]
    public async Task Create_ShouldPersistNonDefaultImage_WhenImageProvided_AndAlso_ShouldUseImageWriter()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();

        var imageWriterMock = this.httpClientFactory.GetImageWriterMock();

        var formData = BuildBookFormWithImage(
            title: "A valid book title long enough",
            shortDescription: "A valid short description",
            longDescription: new string('l', 200),
            authorId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            publishedDate: new DateTime(2000, 1, 1),
            genreIds: []);

        var response = await httpClient.PostAsync("/Books", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBook = await response
            .Content
            .ReadFromJsonAsync<BookDetailsServiceModel>();

        createdBook.Should().NotBeNull();
        createdBook!.ImagePath.Should().NotBe(DefaultImagePath);
        createdBook.ImagePath.Should().StartWith("/images/books/test-");

        imageWriterMock.WriteCalls.Should().Be(1);
        imageWriterMock.LastWrittenPath.Should().NotBeNull();
        imageWriterMock.LastWrittenPath!.Should().StartWith("/images/books/test-");

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == createdBook.Id);

        dbModel.ImagePath.Should().Be(createdBook.ImagePath);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldAutoApprove_WhenAdminCreates()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();

        var formData = BuildBookForm(
            title: "Admin created book title",
            shortDescription: "Admin valid short description",
            longDescription: new string('x', 200),
            authorId: null,
            publishedDate: null,
            genreIds: [Guid.Parse("11111111-1111-1111-1111-111111111111")]);

        var response = await httpClient.PostAsync("/Books", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBook = await response
            .Content
            .ReadFromJsonAsync<BookDetailsServiceModel>();

        createdBook.Should().NotBeNull();
        createdBook.Id.Should().NotBeEmpty();

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == createdBook.Id);

        dbModel.IsApproved.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidModelProvided()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildBookForm(
            title: "a",
            shortDescription: "short",
            longDescription: "too short",
            authorId: null,
            publishedDate: null,
            genreIds: []);

        var response = await httpClient.PostAsync("/Books", formData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response
            .Content
            .Headers
            .ContentType!
            .MediaType
            .Should()
            .Contain("application/problem+json");
    }

    [Fact]
    public async Task Edit_ShouldUpdateImage_AndAlso_ShouldDeleteOldImage_WhenNewImageProvided()
    {
        var bookId = await this.SeedBook(
            imagePath: "/images/books/old.jpg",
            creatorId: "test-user",
            isApproved: true);

        var httpClient = this.httpClientFactory.CreateUserClient();
        var imageWriterMock = this.httpClientFactory.GetImageWriterMock();

        var formData = BuildBookFormWithImage(
            title: "Edited valid title long enough",
            shortDescription: "Edited valid short description",
            longDescription: new string('z', 250),
            authorId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            publishedDate: new DateTime(2010, 2, 2),
            genreIds: [Guid.Parse("11111111-1111-1111-1111-111111111111")]);

        var response = await httpClient.PutAsync(
            $"/Books/{bookId}/",
            formData);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        imageWriterMock.WriteCalls.Should().Be(1);
        imageWriterMock.DeleteCalls.Should().Be(1);
        imageWriterMock.LastDeletedPath.Should().Be("/images/books/old.jpg");
        imageWriterMock.LastWrittenPath.Should().NotBeNull();
        imageWriterMock.LastWrittenPath!.Should().StartWith("/images/books/test-");

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == bookId);

        dbModel.ImagePath.Should().NotBe("/images/books/old.jpg");
        dbModel.ImagePath.Should().StartWith("/images/books/test-");
        dbModel.ModifiedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Edit_ShouldReturnsNoContent_AndAlso_ShouldUpdateFields_WhenBookExists()
    {
        var bookId = await this.SeedBook(
            imagePath: "/images/books/seed.jpg",
            creatorId: "test-user",
            isApproved: true);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildBookForm(
            title: "Edited valid title long enough",
            shortDescription: "Edited valid short description",
            longDescription: new string('z', 250),
            authorId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            publishedDate: new DateTime(2010, 2, 2),
            genreIds: [Guid.Parse("11111111-1111-1111-1111-111111111111")]);

        var response = await httpClient.PutAsync(
            $"/Books/{bookId}/",
            formData);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == bookId);

        dbModel.Title.Should().Be("Edited valid title long enough");
        dbModel.ImagePath.Should().Be("/images/books/seed.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Edit_ShouldReturnBadRequestWithErrorMessage_WhenBookDoesNotExist()
    {
        var nonExistingId = Guid.NewGuid();
        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildBookForm(
            title: "Edited valid title long enough",
            shortDescription: "Edited valid short description",
            longDescription: new string('z', 250),
            authorId: null,
            publishedDate: null,
            genreIds: []);

        var response = await httpClient.PutAsync(
            $"/Books/{nonExistingId}/",
            formData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(json);

        jsonDocument
            .RootElement
            .TryGetProperty("errorMessage", out var message)
            .Should()
            .BeTrue();

        message
            .GetString()
            .Should()
            .Be($"BookDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenBookExists()
    {
        var bookId = await this.SeedBook(
            creatorId: "test-user",
            isApproved: true);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.DeleteAsync(
            $"/Books/{bookId}/");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var count = await data.Books.CountAsync(b => b.Id == bookId);
        count.Should().Be(0);

        var deleted = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == bookId);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequestWithErrorMessage_WhenBookDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync(
            $"/Books/{nonExistingId}/");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(json);

        jsonDocument
            .RootElement
            .TryGetProperty("errorMessage", out var message)
            .Should()
            .BeTrue();

        message
            .GetString()
            .Should()
            .Be($"BookDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Approve_ShouldReturnNoContent_AndAlso_ShouldSetIsApprovedTrue_WhenBookExists()
    {
        var bookId = await this.SeedBook(
            creatorId: "test-user",
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{bookId}/approve/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == bookId);

        dbModel.IsApproved.Should().BeTrue();
    }

    [Fact]
    public async Task Approve_ShouldReturnBadRequestWithErrorMessage_WhenBookDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{nonExistingId}/approve/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(json);

        jsonDocument
            .RootElement
            .TryGetProperty("errorMessage", out var message)
            .Should()
            .BeTrue();

        message
            .GetString()
            .Should()
            .Be($"BookDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Approve_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var bookId = await this.SeedBook(
            creatorId: "test-user",
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{bookId}/approve/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Reject_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenBookExists()
    {
        var bookId = await this.SeedBook(
            creatorId: "test-user",
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{bookId}/reject/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var count = await data.Books.CountAsync(b => b.Id == bookId);
        count.Should().Be(0);

        var deleted = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == bookId);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Reject_ShouldReturnBadRequestWithErrorMessage_WhenBookDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{nonExistingId}/reject/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDocument = JsonDocument.Parse(json);

        jsonDocument
            .RootElement
            .TryGetProperty("errorMessage", out var message)
            .Should()
            .BeTrue();

        message
            .GetString()
            .Should()
            .Be($"BookDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Reject_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var bookId = await this.SeedBook(
            creatorId: "test-user",
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Books/{bookId}/reject/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private static MultipartFormDataContent BuildBookForm(
        string title,
        string shortDescription,
        string longDescription,
        Guid? authorId,
        DateTime? publishedDate,
        ICollection<Guid> genreIds)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(title), "Title" },
            { new StringContent(shortDescription), "ShortDescription" },
            { new StringContent(longDescription), "LongDescription" }
        };

        if (authorId is not null)
        {
            var content = new StringContent(authorId.Value.ToString());
            var name = "AuthorId";

            form.Add(content, name);
        }

        if (publishedDate is not null)
        {
            var content = new StringContent(publishedDate.Value.ToString("O"));
            var name = "PublishedDate";

            form.Add(content, name);
        }

        foreach (var genreId in genreIds)
        {
            var content = new StringContent(genreId.ToString());
            var name = "Genres";

            form.Add(content, name);
        }

        return form;
    }

    private static MultipartFormDataContent BuildBookFormWithImage(
        string title,
        string shortDescription,
        string longDescription,
        Guid? authorId,
        DateTime? publishedDate,
        ICollection<Guid> genreIds)
    {
        var form = BuildBookForm(
            title,
            shortDescription,
            longDescription,
            authorId,
            publishedDate,
            genreIds);

        var bytes = Convert.FromBase64String(MockImageBytes);
        var fileContent = new ByteArrayContent(bytes);

        fileContent
            .Headers
            .ContentType =
            new MediaTypeHeaderValue("image/jpeg");

        form.Add(fileContent, "Image", "test.jpg");

        return form;
    }

    private async Task<Guid> SeedBook(
        double averageRating = 0,
        string imagePath = "/images/books/seed.jpg",
        string? creatorId = "test-user",
        bool isApproved = true,
        Guid? authorId = null,
        ICollection<Guid>? genreIds = null)
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var book = new BookDbModel
        {
            Id = Guid.NewGuid(),
            Title = $"Seed book {Guid.NewGuid():N}",
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            ImagePath = imagePath,
            AverageRating = averageRating,
            RatingsCount = 0,
            PublishedDate = null,
            CreatorId = creatorId,
            IsApproved = isApproved,
            AuthorId = authorId
        };

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var genreIdsDistinct = (genreIds ?? [OtherGenreId])
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        if (genreIdsDistinct.Count == 0)
        {
            genreIdsDistinct.Add(OtherGenreId);
        }

        data
            .BooksGenres
            .AddRange(
                genreIdsDistinct.Select(id => new BookGenreDbModel
                {
                    BookId = book.Id,
                    GenreId = id
                }));

        await data.SaveChangesAsync();

        return book.Id;
    }

    private async Task SeedAuthor(
        Guid id,
        string name)
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var exists = await data
            .Authors
            .AsNoTracking()
            .AnyAsync(a => a.Id == id);

        if (exists)
        {
            return;
        }

        data.Authors.Add(new AuthorDbModel
        {
            Id = id,
            Name = name,
            Biography = new string('b', 120),
            PenName = null,
            Gender = Gender.Other,
            Nationality = Nationality.Bulgaria,
            BornAt = null,
            DiedAt = null,
            ImagePath = "/images/authors/test.jpg",
            IsApproved = true,
            CreatorId = "test-user"
        });

        await data.SaveChangesAsync();
    }

    private async Task SeedGenre(
        Guid id,
        string name = "genre name",
        string imagePath = "images/genres/test.jpg",
        string description = "genre description")
    {
        using var scope = this
         .httpClientFactory
         .Services
         .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var exists = await data
            .Genres
            .AsNoTracking()
            .AnyAsync(g => g.Id == id);

        if (exists)
        {
            return;
        }

        data.Genres.Add(new GenreDbModel
        {
            Id = id,
            Name = name,
            ImagePath = imagePath,
            Description = description
        });

        await data.SaveChangesAsync();
    }

    private async Task SeedUser(
        string id,
        string username,
        string? email = null)
    {
        using var scope = this
           .httpClientFactory
           .Services
           .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var existing = await data
            .Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id);

        if (existing)
        {
            return;
        }

        var normalizedUserName = username.ToUpperInvariant();
        var actualEmail = email ?? $"{username}@test.local";
        var normalizedEmail = actualEmail.ToUpperInvariant();

        var user = new UserDbModel
        {
            Id = id,
            UserName = username,
            NormalizedUserName = normalizedUserName,
            Email = actualEmail,
            NormalizedEmail = normalizedEmail,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
            IsDeleted = false
        };

        data.Users.Add(user);
        await data.SaveChangesAsync();
    }
}
