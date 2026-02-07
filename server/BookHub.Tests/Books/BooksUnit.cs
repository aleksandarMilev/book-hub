namespace BookHub.Tests.Books;

using Areas.Admin.Service;
using Azure;
using BookHub.Data.Models.Shared.BookGenre.Models;
using BookHub.Features.Identity.Data.Models;
using Data;
using Features.Authors.Data.Models;
using Features.Authors.Shared;
using Features.Books.Data.Models;
using Features.Books.Service;
using Features.Books.Service.Models;
using Features.Genres.Data.Models;
using Features.Notifications.Service;
using Features.UserProfile.Service;
using FluentAssertions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NSubstitute;

using static Features.Books.Shared.Constants.Paths;

public sealed class BooksUnit
{
    private static readonly Guid OtherGenreId = new("52e607d4-c347-440a-8d55-cf2e01d88a6c");

    [Fact]
    public async Task TopThree_ShouldReturnThreeBooksOrderedByAverageRatingDesc()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var book1 = NewBookDbModel(
            averageRating: 1.1,
            isApproved: true);

        var book2 = NewBookDbModel(
            averageRating: 4.6,
            isApproved: true);

        var book3 = NewBookDbModel(
            averageRating: 3.8,
            isApproved: true);

        var book4 = NewBookDbModel(
            averageRating: 5.0,
            isApproved: true);

        data.Books.AddRange(
            book1,
            book2,
            book3,
            book4);

        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var result = (await service.TopThree(CancellationToken.None)).ToList();

        result.Should().HaveCount(3);
        result[0].Id.Should().Be(book4.Id);
        result[1].Id.Should().Be(book2.Id);
        result[2].Id.Should().Be(book3.Id);
    }

    [Fact]
    public async Task ByGenre_ShouldReturnPaginatedBooks()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var fantasyId = Guid.NewGuid();
        await SeedGenre(data, OtherGenreId, "Other");
        await SeedGenre(data, fantasyId, "Fantasy");

        var book1 = NewBookDbModel(
            averageRating: 4.0,
            isApproved: true);

        var book2 = NewBookDbModel(
            averageRating: 5.0,
            isApproved: true);

        var book3 = NewBookDbModel(
            averageRating: 3.0,
            isApproved: true);

        data.Books.AddRange(
            book1,
            book2,
            book3);

        await data.SaveChangesAsync();

        data.BooksGenres.AddRange(
            new() { BookId = book1.Id, GenreId = fantasyId },
            new() { BookId = book2.Id, GenreId = fantasyId },
            new() { BookId = book3.Id, GenreId = OtherGenreId });

        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var result = await service.ByGenre(
            fantasyId,
            pageIndex: 1,
            pageSize: 10,
            cancellationToken: CancellationToken.None);

        result.TotalItems.Should().Be(2);
        result.Items.Should().HaveCount(2);
        result.Items.First().AverageRating.Should().Be(5.0);
        result.Items.Select(i => i.Id).Should().Contain([book1.Id, book2.Id]);
    }

    [Fact]
    public async Task ByAuthor_ShouldReturnPaginatedBooks()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var authorId = Guid.NewGuid();
        data.Authors.Add(NewAuthor(authorId));
        await data.SaveChangesAsync();

        var book1 = NewBookDbModel(
            averageRating: 2.0, 
            authorId: authorId,
            isApproved: true);

        var book2 = NewBookDbModel(
            averageRating: 4.0,
            authorId: authorId,
            isApproved: true);

        var book3 = NewBookDbModel(
            averageRating: 5.0,
            authorId: null,
            isApproved: true);

        data.Books.AddRange(
            book1,
            book2,
            book3);

        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var result = await service.ByAuthor(
            authorId,
            pageIndex: 1,
            pageSize: 10,
            cancellationToken: CancellationToken.None);

        result.TotalItems.Should().Be(2);
        result.Items.Should().HaveCount(2);
        result.Items.First().AverageRating.Should().Be(4.0);
        result.Items.Select(i => i.Id).Should().Contain([book1.Id, book2.Id]);
    }

    [Fact]
    public async Task Details_ShouldReturnNull_WhenBookWithSuchIdNotInTheDb()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var service = NewBooksService(data, currentUserService);

        var result = await service.Details(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task Create_ShouldSetDefaultImagePath_AndAlso_ShouldPersistBookInDb_AndAlso_ShouldSetCreatorId_AndAlso_ShouldNotApprove_WhenNonAdmin_AndAlso_ShouldMapGenres_AndAlso_ShouldFallbackToOtherGenre_WhenNoGenresProvided()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedUser(data, "user-1", "shano"); 
        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = Substitute.For<IAdminService>();
        adminService.GetId().Returns("admin-1");

        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                Arg.Any<string>(),
                Arg.Any<IImageDdModel>(),
                Arg.Any<IImageServiceModel>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                var defaultPath = (string?)callInfo[3];

                dbModel.ImagePath = defaultPath!;
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);

        var serviceModel = new CreateBookServiceModel
        {
            Title = "A valid book title",
            AuthorId = null,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = new DateTime(2000, 1, 1),
            Genres = []
        };

        var created = await service.Create(serviceModel);

        created.Id.Should().NotBeEmpty();
        created.ImagePath.Should().Be(DefaultImagePath);

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == created.Id);

        dbModel.CreatorId.Should().Be("user-1");
        dbModel.IsApproved.Should().BeFalse();
        dbModel.ImagePath.Should().Be(DefaultImagePath);
        dbModel.CreatedOn.Should().NotBe(default);

        var maps = await data
            .BooksGenres
            .AsNoTracking()
            .Where(bg => bg.BookId == created.Id)
            .ToListAsync();

        maps.Should().HaveCount(1);
        maps[0].GenreId.Should().Be(OtherGenreId);

        await notificationService
            .Received(1)
            .CreateOnBookCreation(
                created.Id,
                created.Title,
                "admin-1",
                Arg.Any<CancellationToken>());

        await imageWriter
            .Received(1)
            .Write(
                ImagePathPrefix,
                Arg.Any<IImageDdModel>(),
                Arg.Any<IImageServiceModel>(),
                DefaultImagePath,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldApprove_WhenAdmin()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        await SeedUser(data, "admin-1", "admin");
        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                Arg.Any<string>(),
                Arg.Any<IImageDdModel>(),
                Arg.Any<IImageServiceModel>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                var defaultPath = (string?)callInfo[3];
                dbModel.ImagePath = defaultPath!;
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);

        var serviceModel = new CreateBookServiceModel
        {
            Title = "Admin book title",
            AuthorId = null,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = []
        };

        var created = await service.Create(serviceModel);

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == created.Id);

        dbModel.IsApproved.Should().BeTrue();

        await notificationService
            .DidNotReceiveWithAnyArgs()
            .CreateOnBookCreation(
                bookId: default,
                bookTitle: default!,
                receiverId: default!,
                cancellationToken: default);
    }

    [Fact]
    public async Task Edit_ShouldReturnNotFoundResult_WhenBookWithSuchIdNotInTheDb()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var service = NewBooksService(data, currentUserService);

        var nonExistingId = Guid.NewGuid();
        var serviceModel = new CreateBookServiceModel
        {
            Title = "Updated title",
            AuthorId = null,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = []
        };

        var result = await service.Edit(nonExistingId, serviceModel);

        result.Succeeded.Should().BeFalse();
        result
            .ErrorMessage
            .Should()
            .Be($"BookDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Edit_ShouldReturnUnauthorizedResult_WhenNotCreator()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        currentUserService.GetId().Returns("user-2");

        await SeedGenre(data, OtherGenreId, "Other");

        var book = NewBookDbModel(
            creatorId: "user-1",
            imagePath: "/images/books/old.jpg");

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var serviceModel = new CreateBookServiceModel
        {
            Title = "Updated title",
            AuthorId = null,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = []
        };

        var result = await service.Edit(book.Id, serviceModel);

        result.Succeeded.Should().BeFalse();
        result
            .ErrorMessage
            .Should()
            .Be($"User with Id: user-2 can not modify BookDbModel with Id: {book.Id}!");
    }

    [Fact]
    public async Task Edit_ShouldChangeImagePath_AndAlso_ShouldDeletesOldImage_WhenNewImageProvided()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedUser(data, "user-1", "shano");
        await SeedGenre(data, OtherGenreId, "Other");

        var fantasyId = Guid.NewGuid();
        await SeedGenre(data, fantasyId, "Fantasy");

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                Arg.Any<string>(),
                Arg.Any<IImageDdModel>(),
                Arg.Any<IImageServiceModel>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                dbModel.ImagePath = "/images/books/new.jpg";
            });

        imageWriter
            .Delete(
                resourceName: Arg.Any<string>(),
                imagePath: Arg.Any<string?>(),
                defaultImagePath: Arg.Any<string?>())
            .Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();
        var logger = Substitute.For<ILogger<BookService>>();

        var service = new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);

        var book = NewBookDbModel(
            creatorId: "user-1",
            imagePath: "/images/books/old.jpg",
            isApproved: true);

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var dummyFile = new FormFile(
            baseStream: new MemoryStream([1, 2, 3]),
            baseStreamOffset: 0,
            length: 3,
            name: "Image",
            fileName: "test.jpg");

        var serviceModel = new CreateBookServiceModel
        {
            Title = "Updated title is valid",
            AuthorId = null,
            Image = dummyFile,
            ShortDescription = "Updated short description",
            LongDescription = new string('u', 200),
            PublishedDate = new DateTime(2011, 1, 1),
            Genres = [fantasyId]
        };

        var result = await service.Edit(book.Id, serviceModel);

        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == book.Id);

        dbModel.Title.Should().Be(serviceModel.Title);
        dbModel.ImagePath.Should().Be("/images/books/new.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();

        imageWriter
            .Received(1)
            .Delete(
                nameof(BookDbModel),
                "/images/books/old.jpg",
                DefaultImagePath);

        var mapEntities = await data
            .BooksGenres
            .AsNoTracking()
            .Where(bg => bg.BookId == book.Id)
            .ToListAsync();

        mapEntities.Should().HaveCount(1);
        mapEntities[0].GenreId.Should().Be(fantasyId);
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteBook_AndAlso_ShouldFilterItOut()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        currentUserService.GetId().Returns("admin-1");
        currentUserService.IsAdmin().Returns(true);

        await SeedGenre(data, OtherGenreId, "Other");

        var service = NewBooksService(data, currentUserService);

        var book = NewBookDbModel(
            creatorId: "user-1",
            isApproved: true);

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var result = await service.Delete(book.Id);
        result.Succeeded.Should().BeTrue();

        var count = await data.Books.CountAsync(b => b.Id == book.Id);
        count.Should().Be(0);

        var deleted = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == book.Id);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Approve_ShouldReturnUnauthorizedResult_WhenNotAdmin()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var service = NewBooksService(data, currentUserService);

        var bookId = Guid.NewGuid();

        var result = await service.Approve(bookId);
        result.Succeeded.Should().BeFalse();
        result
            .ErrorMessage
            .Should()
            .Be($"BookDbModel with Id: {bookId} was not found!");
    }

    [Fact]
    public async Task Approve_ShouldSetIsApprovedTrue_AndAlso_ShouldNotifyCreator_AndAlso_ShouldIncrementCreatedBooksCount()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        currentUserService.GetId().Returns("admin-1");
        currentUserService.IsAdmin().Returns(true);

        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<BookService>>();

        var book = NewBookDbModel(
            creatorId: "user-1",
            isApproved: false);

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var service = new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);

        var result = await service.Approve(book.Id);
        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == book.Id);

        dbModel.IsApproved.Should().BeTrue();

        await notificationService
            .Received(1)
            .CreateOnBookApproved(
                book.Id,
                book.Title,
                "user-1",
                Arg.Any<CancellationToken>());

        await profileService
            .Received(1)
            .IncrementCreatedBooksCount(
                "user-1",
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Reject_ShouldSoftDeleteBook_AndAlso_ShouldNotifyCreator()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        currentUserService.GetId().Returns("admin-1");
        currentUserService.IsAdmin().Returns(true);

        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<BookService>>();

        var book = NewBookDbModel(creatorId: "user-1", isApproved: false);
        data.Books.Add(book);
        await data.SaveChangesAsync();

        var service = new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);

        var result = await service.Reject(book.Id);
        result.Succeeded.Should().BeTrue();

        var count = await data.Books.CountAsync(b => b.Id == book.Id);
        count.Should().Be(0);

        var deleted = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == book.Id);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();

        await notificationService
            .Received(1)
            .CreateOnBookRejected(
                book.Id,
                book.Title,
                "user-1",
                Arg.Any<CancellationToken>());
    }

    private static async Task<(
        BookHubDbContext Data,
        ICurrentUserService CurrentUserService,
        SqliteConnection SqliteConnection)>
    CreateSqliteDb(
        string userId = "user-1",
        string username = "shano",
        bool isAdmin = false)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseSqlite(connection)
            .Options;

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.GetId().Returns(userId);
        currentUserService.GetUsername().Returns(username);
        currentUserService.IsAdmin().Returns(isAdmin);

        var data = new BookHubDbContext(options, currentUserService);
        await data.Database.EnsureCreatedAsync();

        await SeedUser(data, userId, username);

        return (data, currentUserService, connection);
    }

    private static BookService NewBooksService(
        BookHubDbContext data,
        ICurrentUserService currentUserService)
    {
        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<BookService>>();
        var profileService = Substitute.For<IProfileService>();

        return new BookService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            logger,
            profileService);
    }

    private static BookDbModel NewBookDbModel(
        Guid? id = null,
        double averageRating = 0,
        string title = "A valid book title",
        string shortDescription = "A valid short description",
        string? longDescription = null,
        string imagePath = "/images/books/old.jpg",
        Guid? authorId = null,
        string? creatorId = "user-1",
        bool isApproved = false)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            ShortDescription = shortDescription,
            LongDescription = longDescription ?? new string('l', 200),
            AverageRating = averageRating,
            RatingsCount = 0,
            PublishedDate = null,
            ImagePath = imagePath,
            AuthorId = authorId,
            CreatorId = creatorId,
            IsApproved = isApproved
        };

    private static AuthorDbModel NewAuthor(Guid id)
        => new()
        {
            Id = id,
            Name = "Seed author",
            Biography = new string('b', 120),
            PenName = null,
            Gender = Gender.Other,
            Nationality = Nationality.Bulgaria,
            BornAt = null,
            DiedAt = null,
            ImagePath = "/images/authors/seed.jpg",
            IsApproved = true,
            CreatorId = "user-1"
        };

    private static async Task SeedGenre(
        BookHubDbContext data,
        Guid id,
        string name = "genre name",
        string imagePath = "images/genres/test.jpg",
        string description = "genre description")
    {
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

    private static async Task SeedUser(
        BookHubDbContext data,
        string id,
        string username,
        string? email = null)
    {
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
