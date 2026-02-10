namespace BookHub.Tests.Genres;

using Data;
using Data.Models.Shared.BookGenre.Models;
using Features.Books.Data.Models;
using Features.Genres.Data.Models;
using Features.Genres.Service;
using FluentAssertions;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public sealed class GenresUnit
{
    [Fact]
    public async Task Names_ShouldReturnAllGenresAsNamesServiceModels_AndAlso_ShouldNotReturnDeleted()
    {
        var data = CreateInMemoryDb();

        var genre1 = NewGenre(name: "Fantasy");
        var genre2 = NewGenre(name: "Sci-Fi");

        var deleted = NewGenre(name: "Deleted");
        deleted.IsDeleted = true;
        deleted.DeletedOn = DateTime.UtcNow;

        data.Genres.AddRange(genre1, genre2, deleted);
        await data.SaveChangesAsync();

        var service = new GenreService(data);

        var result = (await service.Names()).ToList();

        result.Select(g => g.Id).Should().Contain([genre1.Id, genre2.Id]);
        result.Select(g => g.Id).Should().NotContain(deleted.Id);
        result.Select(g => g.Name).Should().Contain(["Fantasy", "Sci-Fi"]);
        result.Select(g => g.Name).Should().NotContain("Deleted");
    }

    [Fact]
    public async Task Details_ShouldReturnNull_WhenGenreWithSuchIdNotInTheDb()
    {
        var data = CreateInMemoryDb();

        var service = new GenreService(data);

        var result = await service.Details(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task Details_ShouldReturnNull_WhenGenreIsDeleted()
    {
        var data = CreateInMemoryDb();

        var deletedGenre = NewGenre(name: "Deleted");
        deletedGenre.IsDeleted = true;
        deletedGenre.DeletedOn = DateTime.UtcNow;

        data.Genres.Add(deletedGenre);
        await data.SaveChangesAsync();

        var service = new GenreService(data);

        var result = await service.Details(deletedGenre.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Details_ShouldReturnGenreDetails_AndAlso_ShouldReturnTopThreeBooksOrderedByAverageRatingDesc()
    {
        var data = CreateInMemoryDb();

        var genre = NewGenre(
            name: "Mystery",
            description: "Mystery genre description long enough",
            imagePath: "/images/genres/mystery.jpg");

        data.Genres.Add(genre);
        await data.SaveChangesAsync();

        var book1 = NewBook(
            averageRating: 4.6,
            title: "B1");

        var book2 = NewBook(
            averageRating: 3.8,
            title: "B2");

        var book3 = NewBook(
            averageRating: 5.0,
            title: "B3");

        var book4 = NewBook(
            averageRating: 2.1,
            title: "B4");

        data.Books.AddRange(book1, book2, book3, book4);
        await data.SaveChangesAsync();

        data.BooksGenres.AddRange(
            NewBookGenre(book1.Id, genre.Id),
            NewBookGenre(book2.Id, genre.Id),
            NewBookGenre(book3.Id, genre.Id),
            NewBookGenre(book4.Id, genre.Id));

        await data.SaveChangesAsync();

        var service = new GenreService(data);

        var result = await service.Details(genre.Id);
        result.Should().NotBeNull();
        result.Id.Should().Be(genre.Id);
        result.Name.Should().Be("Mystery");
        result.Description.Should().Be("Mystery genre description long enough");
        result.ImagePath.Should().Be("/images/genres/mystery.jpg");
        result.TopBooks.Should().NotBeNull();

        var topBooks = result.TopBooks.ToList();
        topBooks.Should().HaveCount(3);

        topBooks[0].Id.Should().Be(book3.Id);
        topBooks[1].Id.Should().Be(book1.Id);
        topBooks[2].Id.Should().Be(book2.Id);

        topBooks[0].AverageRating.Should().Be(5.0);
        topBooks[1].AverageRating.Should().Be(4.6);
        topBooks[2].AverageRating.Should().Be(3.8);

        topBooks
            .SelectMany(b => b.Genres.Select(g => g.Id))
            .Should()
            .Contain(genre.Id);
    }

    private static BookHubDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseInMemoryDatabase($"BookHubTests_Authors_{Guid.NewGuid():N}")
            .Options;

        var currentUserService = Substitute.For<ICurrentUserService>();
        return new(options, currentUserService);
    }

    private static GenreDbModel NewGenre(
        Guid? id = null,
        string name = "Genre",
        string imagePath = "/images/genres/seed.jpg",
        string? description = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            ImagePath = imagePath,
            Description = description ?? new string('d', 30)
        };

    private static BookDbModel NewBook(
        Guid? id = null,
        double averageRating = 0,
        string title = "Book title")
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            AverageRating = averageRating,
            RatingsCount = 0,
            PublishedDate = null,
            ImagePath = "/images/books/seed.jpg",
            AuthorId = null,
            CreatorId = "user-1",
            IsApproved = true
        };

    private static BookGenreDbModel NewBookGenre(
        Guid bookId,
        Guid genreId)
        => new()
        {
            BookId = bookId,
            GenreId = genreId
        };
}
