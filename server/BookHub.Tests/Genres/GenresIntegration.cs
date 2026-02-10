namespace BookHub.Tests.Genres;

using System.Net;
using System.Net.Http.Json;
using Data;
using Data.Models.Shared.BookGenre.Models;
using Features.Books.Data.Models;
using Features.Genres.Data.Models;
using Features.Genres.Service.Models;
using Features.Identity.Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class GenresIntegration : IAsyncLifetime
{
    private readonly GenresWebApplicationFactory httpClientFactory = new();

    public async Task InitializeAsync()
    {
        await this
            .httpClientFactory
            .ResetDatabase();

        await this.SeedUser("test-user", "user");
        await this.SeedUser("test-admin-id", "admin");
    }

    public Task DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Names_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var httpClient = this.httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync("/Genres/");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Details_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var httpClient = this.httpClientFactory.CreateClient();
        var id = Guid.NewGuid();

        var response = await httpClient.GetAsync($"/Genres/{id}/");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Names_ShouldReturnOk_AndAlso_ShouldReturnAllGenres_AsNameServiceModels_AndAlso_ShouldNotReturnDeleted()
    {
        var genre1Id = await this.SeedGenre(
            name: "Fantasy",
            description: "Fantasy genre description long enough",
            imagePath: "/images/genres/fantasy.jpg");

        var genre2Id = await this.SeedGenre(
            name: "Sci-Fi",
            description: "Sci-fi genre description long enough",
            imagePath: "/images/genres/scifi.jpg");

        var deletedGenreId = await this.SeedGenre(
            name: "Deleted",
            description: "Deleted genre description long enough",
            imagePath: "/images/genres/deleted.jpg");

        await this.SoftDeleteGenre(deletedGenreId);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.GetAsync("/Genres/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<GenreNameServiceModel>>();

        body.Should().NotBeNull();

        var list = body.ToList();
        list.Select(g => g.Id).Should().Contain([genre1Id, genre2Id]);
        list.Select(g => g.Id).Should().NotContain(deletedGenreId);

        list.Select(g => g.Name).Should().Contain(["Fantasy", "Sci-Fi"]);
        list.Select(g => g.Name).Should().NotContain("Deleted");
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenNoGenreWithSuchIdInDb()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.GetAsync($"/Genres/{nonExistingId}/");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenGenreIsDeleted()
    {
        var genreId = await this.SeedGenre(
            name: "Horror",
            description: "Horror genre description long enough",
            imagePath: "/images/genres/horror.jpg");

        await this.SoftDeleteGenre(genreId);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.GetAsync($"/Genres/{genreId}/");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Details_ShouldReturnGenreDetails_AndAlso_ShouldReturnTopThreeBooksOrderedByAverageRatingDesc()
    {
        var genreId = await this.SeedGenre(
            name: "Mystery",
            description: "Mystery genre description long enough",
            imagePath: "/images/genres/mystery.jpg");

        var book1Id = await this.SeedBookWithGenre(
            genreId,
            averageRating: 4.6,
            title: "B1");

        var book2Id = await this.SeedBookWithGenre(
            genreId,
            averageRating: 3.8,
            title: "B2");

        var book3Id = await this.SeedBookWithGenre(
            genreId,
            averageRating: 5.0,
            title: "B3");

        var _ = await this.SeedBookWithGenre(
            genreId,
            averageRating: 2.1,
            title: "B4");

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.GetAsync($"/Genres/{genreId}/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<GenreDetailsServiceModel>();

        body.Should().NotBeNull();
        body.Id.Should().Be(genreId);
        body.Name.Should().Be("Mystery");
        body.Description.Should().Be("Mystery genre description long enough");
        body.ImagePath.Should().Be("/images/genres/mystery.jpg");

        body.TopBooks.Should().NotBeNull();

        var topBooks = body.TopBooks.ToList();
        topBooks.Should().HaveCount(3);

        topBooks[0].Id.Should().Be(book3Id);
        topBooks[1].Id.Should().Be(book1Id);
        topBooks[2].Id.Should().Be(book2Id);

        topBooks[0].AverageRating.Should().Be(5.0);
        topBooks[1].AverageRating.Should().Be(4.6);
        topBooks[2].AverageRating.Should().Be(3.8);
    }

    private async Task<Guid> SeedGenre(
        string name,
        string description,
        string imagePath)
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var genre = new GenreDbModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            ImagePath = imagePath
        };

        data.Genres.Add(genre);
        await data.SaveChangesAsync();

        return genre.Id;
    }

    private async Task<Guid> SeedBookWithGenre(
        Guid genreId,
        double averageRating,
        string title)
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
            Title = title,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            ImagePath = "/images/books/seed.jpg",
            AverageRating = averageRating,
            RatingsCount = 0,
            PublishedDate = null,
            CreatorId = "test-user",
            IsApproved = true,
            AuthorId = null
        };

        data.Books.Add(book);
        await data.SaveChangesAsync();

        data.BooksGenres.Add(new BookGenreDbModel
        {
            BookId = book.Id,
            GenreId = genreId
        });

        await data.SaveChangesAsync();

        return book.Id;
    }

    private async Task SoftDeleteGenre(Guid genreId)
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var genre = await data
            .Genres
            .IgnoreQueryFilters()
            .SingleAsync(g => g.Id == genreId);

        genre.IsDeleted = true;
        genre.DeletedOn = DateTime.UtcNow;

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
