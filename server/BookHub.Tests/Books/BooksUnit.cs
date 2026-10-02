namespace BookHub.Tests.Books;

using Areas.Admin.Service;
using Data;
using Features.Authors.Data.Models;
using Features.Authors.Shared;
using Features.Books.Data.Models;
using Features.Books.Service;
using Features.Books.Service.Models;
using Features.Genres.Data.Models;
using Features.Identity.Data.Models;
using Features.Notifications.Service;
using Features.UserProfile.Service;
using FluentAssertions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models;
using Infrastructure.Services.PageClamper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Mocks;
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

        var adminService = new AdminServiceMock("admin-1");

        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                var defaultPath = (string?)callInfo[3];

                dbModel.ImagePath = defaultPath!;
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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

        var created = (await service.Create(serviceModel)).Data!;

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

        notificationService
            .Received(1)
            .AddOnBookCreation(
                created.Id,
                created.Title,
                Arg.Is<IEnumerable<string>>(ids => ids.SequenceEqual(new[] { "admin-1" })));

        await imageWriter
            .Received(1)
            .Write(
                resourceName: ImagePathPrefix,
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: DefaultImagePath,
                cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Create_ShouldSetAuthorIdNull_WhenAuthorDoesNotExist()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = new AdminServiceMock("admin-1");

        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();
        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                var defaultPath = (string?)callInfo[3];

                dbModel.ImagePath = defaultPath!;
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            profileService);

        var nonExistingAuthorId = Guid.NewGuid();

        var serviceModel = new CreateBookServiceModel
        {
            Title = "A valid book title",
            AuthorId = nonExistingAuthorId,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = []
        };

        var created = (await service.Create(serviceModel)).Data!;
        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == created.Id);

        dbModel.AuthorId.Should().BeNull();
    }


    [Fact]
    public async Task Create_ShouldSetNonDefaultImagePath_WhenImageProvided()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var adminService = new AdminServiceMock("admin-1");

        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                dbModel.ImagePath = "/images/books/new.jpg";
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
            profileService);

        var dummyFile = new FormFile(
            baseStream: new MemoryStream([1, 2, 3]),
            baseStreamOffset: 0,
            length: 3,
            name: "Image",
            fileName: "test.jpg");

        var serviceModel = new CreateBookServiceModel
        {
            Title = "A valid book title",
            AuthorId = null,
            Image = dummyFile,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = []
        };

        var created = (await service.Create(serviceModel)).Data!;

        created.ImagePath.Should().Be("/images/books/new.jpg");
        created.ImagePath.Should().NotBe(DefaultImagePath);

        await imageWriter
            .Received(1)
            .Write(
                resourceName: ImagePathPrefix,
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: DefaultImagePath,
                cancellationToken: Arg.Any<CancellationToken>());
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
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                var defaultPath = (string?)callInfo[3];
                dbModel.ImagePath = defaultPath!;
            });

        var logger = Substitute.For<ILogger<BookService>>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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

        var created = (await service.Create(serviceModel)).Data!;

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .SingleAsync(b => b.Id == created.Id);

        dbModel.IsApproved.Should().BeTrue();

        notificationService
            .DidNotReceiveWithAnyArgs()
            .AddOnBookCreation(
                bookId: default,
                bookTitle: default!,
                receiverIds: default!);
    }

    [Fact]
    public async Task Edit_ShouldSetPendingAuthorId_AndAlso_ShouldNotChangeBook_WhenNonAdminAndAuthorExists()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var authorId = Guid.NewGuid();
        data.Authors.Add(NewAuthor(authorId));
        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var book = NewBookDbModel(
            creatorId: "user-1",
            isApproved: true);

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var serviceModel = new CreateBookServiceModel
        {
            Title = "Updated title is valid",
            AuthorId = authorId,
            Image = null,
            ShortDescription = "Updated short description",
            LongDescription = new string('u', 200),
            PublishedDate = null,
            Genres = []
        };

        var result = await service.Edit(book.Id, serviceModel);
        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .AsNoTracking()
            .SingleAsync(b => b.Id == book.Id);

        dbModel.AuthorId.Should().BeNull();
        dbModel.Title.Should().Be(book.Title);

        var pending = await data
            .BookEdits
            .AsNoTracking()
            .SingleAsync(e => e.BookId == book.Id);

        pending.AuthorId.Should().Be(authorId);
        pending.Title.Should().Be(serviceModel.Title);
        pending.RequestedById.Should().Be("user-1");
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
    public async Task Edit_ShouldCreatePendingEditWithNewImage_AndAlso_ShouldNotChangeBook_AndAlso_ShouldNotifyAdmins_WhenNonAdmin()
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
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
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

        var adminService = new AdminServiceMock("admin-1", "admin-2");
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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
            .AsNoTracking()
            .SingleAsync(b => b.Id == book.Id);

        dbModel.Title.Should().Be(book.Title);
        dbModel.ImagePath.Should().Be("/images/books/old.jpg");

        var pending = await data
            .BookEdits
            .AsNoTracking()
            .SingleAsync(e => e.BookId == book.Id);

        pending.Title.Should().Be(serviceModel.Title);
        pending.ImagePath.Should().Be("/images/books/new.jpg");
        pending.GenresJson.Should().Contain(fantasyId.ToString());

        await imageWriter
            .Received(1)
            .Write(
                resourceName: PendingImagePathPrefix,
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: null,
                cancellationToken: Arg.Any<CancellationToken>());

        imageWriter
            .DidNotReceiveWithAnyArgs()
            .Delete(
                resourceName: default!,
                imagePath: default,
                defaultImagePath: default);

        var mapEntities = await data
            .BooksGenres
            .AsNoTracking()
            .Where(bg => bg.BookId == book.Id)
            .ToListAsync();

        mapEntities.Should().BeEmpty();

        notificationService
            .Received(1)
            .AddOnBookEdition(
                book.Id,
                book.Title,
                Arg.Is<IEnumerable<string>>(ids => ids.SequenceEqual(new[] { "admin-1", "admin-2" })));
    }

    [Fact]
    public async Task Edit_ShouldCallImageWriterWithPendingPrefixAndNullDefaultImagePath_WhenNonAdmin()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var imageWriter = Substitute.For<IImageWriter>();
        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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
            PublishedDate = null,
            Genres = []
        };

        var result = await service.Edit(book.Id, serviceModel);

        result.Succeeded.Should().BeTrue();

        await imageWriter
           .Received(1)
           .Write(
               resourceName: PendingImagePathPrefix,
               dbModel: Arg.Any<IImageDdModel>(),
               serviceModel: Arg.Any<IImageServiceModel>(),
               defaultImagePath: null,
               cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Edit_ShouldNotDeleteOldImage_WhenNewImageProvided_ButImagePathDoesNotChange()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
             .When(writer => writer.Write(
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                dbModel.ImagePath = "/images/books/old.jpg";
            });

        var adminService = Substitute.For<IAdminService>();
        var notificationService = Substitute.For<INotificationService>();
        var profileService = Substitute.For<IProfileService>();
        var logger = Substitute.For<ILogger<BookService>>();

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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
            PublishedDate = null,
            Genres = []
        };

        var result = await service.Edit(book.Id, serviceModel);
        result.Succeeded.Should().BeTrue();

        imageWriter
            .DidNotReceive()
            .Delete(
                resourceName: Arg.Any<string>(),
                imagePath: Arg.Any<string?>(),
                defaultImagePath: Arg.Any<string?>());
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

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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

        var service = NewBooksService(
            data,
            currentUserService,
            adminService,
            notificationService,
            imageWriter,
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

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Create_ShouldPersistBook_AndAlso_ShouldNotifyEveryAdmin_WhenNonAdmin(int adminCount)
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var adminIds = Enumerable
            .Range(1, adminCount)
            .Select(i => $"admin-{i}")
            .ToArray();

        foreach (var adminId in adminIds)
        {
            await SeedUser(data, adminId, adminId);
        }

        var service = NewBooksService(
            data,
            currentUserService,
            adminService: new AdminServiceMock(adminIds),
            notificationService: NewNotificationService(data, currentUserService),
            imageWriter: new ImageWriterMock());

        var result = await service.Create(NewCreateBookServiceModel());

        result.Succeeded.Should().BeTrue();

        var bookExists = await data
            .Books
            .IgnoreQueryFilters()
            .AnyAsync(b => b.Id == result.Data!.Id);

        bookExists.Should().BeTrue();

        var receivers = await data
            .Notifications
            .Where(n => n.ResourceId == result.Data!.Id)
            .Select(n => n.ReceiverId)
            .ToListAsync();

        receivers.Should().BeEquivalentTo(adminIds);
    }

    [Fact]
    public async Task Create_ShouldNotPersistBook_WhenAdminNotificationCannotBeSaved()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        // No user row for this admin ID, so the notification violates its FK.
        var service = NewBooksService(
            data,
            currentUserService,
            adminService: new AdminServiceMock("missing-admin"),
            notificationService: NewNotificationService(data, currentUserService),
            imageWriter: new ImageWriterMock());

        var serviceModel = NewCreateBookServiceModel();

        var act = () => service.Create(serviceModel);

        await act.Should().ThrowAsync<DbUpdateException>();

        var bookExists = await data
            .Books
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AnyAsync(b => b.Title == serviceModel.Title);

        bookExists.Should().BeFalse();
    }

    [Fact]
    public async Task Create_ShouldReturnError_AndAlso_ShouldNotPersistBook_AndAlso_ShouldNotWriteImage_WhenGenreDoesNotExist()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var imageWriter = new ImageWriterMock();
        var service = NewBooksService(
            data,
            currentUserService,
            imageWriter: imageWriter);

        var unknownGenreId = Guid.NewGuid();
        var serviceModel = NewCreateBookServiceModel(genres: [OtherGenreId, unknownGenreId]);

        var result = await service.Create(serviceModel);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Genres with Id(s): {unknownGenreId} were not found!");

        imageWriter.WriteCalls.Should().Be(0);

        var anyBook = await data
            .Books
            .IgnoreQueryFilters()
            .AnyAsync();

        anyBook.Should().BeFalse();
    }

    [Fact]
    public async Task Create_ShouldMapOtherGenre_AndAlso_ShouldNotMutateCallersGenres_WhenNoGenresProvided()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        await SeedGenre(data, OtherGenreId, "Other");

        var service = NewBooksService(
            data,
            currentUserService,
            imageWriter: new ImageWriterMock());

        var serviceModel = NewCreateBookServiceModel(genres: []);

        var result = await service.Create(serviceModel);

        result.Succeeded.Should().BeTrue();
        serviceModel.Genres.Should().BeEmpty();

        var genreIds = await data
            .BooksGenres
            .Where(bg => bg.BookId == result.Data!.Id)
            .Select(bg => bg.GenreId)
            .ToListAsync();

        genreIds.Should().Equal(OtherGenreId);
    }

    [Fact]
    public async Task Create_ShouldPersistBookWithoutGenres_WhenNoGenresProvided_AndOtherGenreDoesNotExist()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var service = NewBooksService(
            data,
            currentUserService,
            imageWriter: new ImageWriterMock());

        var result = await service.Create(NewCreateBookServiceModel(genres: []));

        result.Succeeded.Should().BeTrue();

        var bookExists = await data
            .Books
            .IgnoreQueryFilters()
            .AnyAsync(b => b.Id == result.Data!.Id);

        bookExists.Should().BeTrue();

        var anyGenreMap = await data
            .BooksGenres
            .AnyAsync(bg => bg.BookId == result.Data!.Id);

        anyGenreMap.Should().BeFalse();
    }

    [Fact]
    public async Task Edit_ShouldReturnError_AndAlso_ShouldNotCreatePendingEdit_WhenGenreDoesNotExist()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var book = NewBookDbModel(
            creatorId: "user-1",
            isApproved: true);

        data.Books.Add(book);
        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var unknownGenreId = Guid.NewGuid();
        var result = await service.Edit(
            book.Id,
            NewCreateBookServiceModel(genres: [unknownGenreId]));

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Genres with Id(s): {unknownGenreId} were not found!");

        var hasPendingEdit = await data
            .BookEdits
            .AnyAsync(e => e.BookId == book.Id);

        hasPendingEdit.Should().BeFalse();
    }

    [Fact]
    public async Task Edit_ShouldApplyChangesDirectly_AndAlso_ShouldRemapGenres_AndAlso_ShouldDeleteOldImage_AndAlso_ShouldNotCreatePendingEdit_WhenAdmin()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        await SeedUser(data, "user-1", "shano");
        await SeedGenre(data, OtherGenreId, "Other");

        var fantasyId = Guid.NewGuid();
        await SeedGenre(data, fantasyId, "Fantasy");

        var authorId = Guid.NewGuid();
        data.Authors.Add(NewAuthor(authorId));

        var book = NewBookDbModel(
            creatorId: "user-1",
            imagePath: "/images/books/old.jpg",
            isApproved: true);

        data.Books.Add(book);
        data.BooksGenres.Add(new() { BookId = book.Id, GenreId = OtherGenreId });
        await data.SaveChangesAsync();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writer => writer.Write(
                resourceName: Arg.Any<string>(),
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: Arg.Any<string?>(),
                cancellationToken: Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                dbModel.ImagePath = "/images/books/new.jpg";
            });

        var notificationService = Substitute.For<INotificationService>();
        var service = NewBooksService(
            data,
            currentUserService,
            adminService: new AdminServiceMock("admin-1"),
            notificationService: notificationService,
            imageWriter: imageWriter);

        var dummyFile = new FormFile(
            baseStream: new MemoryStream([1, 2, 3]),
            baseStreamOffset: 0,
            length: 3,
            name: "Image",
            fileName: "test.jpg");

        var serviceModel = new CreateBookServiceModel
        {
            Title = "Admin edited title",
            AuthorId = authorId,
            Image = dummyFile,
            ShortDescription = "Admin edited short description",
            LongDescription = new string('a', 200),
            PublishedDate = new DateTime(2011, 1, 1),
            Genres = [fantasyId]
        };

        var result = await service.Edit(book.Id, serviceModel);

        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .AsNoTracking()
            .SingleAsync(b => b.Id == book.Id);

        dbModel.Title.Should().Be("Admin edited title");
        dbModel.AuthorId.Should().Be(authorId);
        dbModel.ImagePath.Should().Be("/images/books/new.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();

        await imageWriter
            .Received(1)
            .Write(
                resourceName: ImagePathPrefix,
                dbModel: Arg.Any<IImageDdModel>(),
                serviceModel: Arg.Any<IImageServiceModel>(),
                defaultImagePath: null,
                cancellationToken: Arg.Any<CancellationToken>());

        imageWriter
            .Received(1)
            .Delete(
                ImagePathPrefix,
                "/images/books/old.jpg",
                DefaultImagePath);

        var genreIds = await data
            .BooksGenres
            .Where(bg => bg.BookId == book.Id)
            .Select(bg => bg.GenreId)
            .ToListAsync();

        genreIds.Should().Equal(fantasyId);

        var hasPendingEdit = await data
            .BookEdits
            .AnyAsync(e => e.BookId == book.Id);

        hasPendingEdit.Should().BeFalse();

        notificationService
            .DidNotReceiveWithAnyArgs()
            .AddOnBookEdition(
                bookId: default,
                bookTitle: default!,
                receiverIds: default!);
    }

    [Fact]
    public async Task Approve_ShouldDropGenresThatNoLongerExist_AndAlso_ShouldFallBackToOtherGenre()
    {
        var (data, currentUserService, connection) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        await SeedUser(data, "user-1", "shano");
        await SeedGenre(data, OtherGenreId, "Other");

        var book = NewBookDbModel(
            creatorId: "user-1",
            isApproved: true);

        data.Books.Add(book);
        data.BookEdits.Add(new BookEditDbModel
        {
            BookId = book.Id,
            RequestedById = "user-1",
            Title = "Pending title",
            ShortDescription = "Pending short description",
            LongDescription = new string('p', 200),
            ImagePath = book.ImagePath,
            GenresJson = $"[\"{Guid.NewGuid()}\"]"
        });

        await data.SaveChangesAsync();

        var service = NewBooksService(data, currentUserService);

        var result = await service.Approve(book.Id);

        result.Succeeded.Should().BeTrue();

        var genreIds = await data
            .BooksGenres
            .Where(bg => bg.BookId == book.Id)
            .Select(bg => bg.GenreId)
            .ToListAsync();

        genreIds.Should().Equal(OtherGenreId);
    }

    private static NotificationService NewNotificationService(
        BookHubDbContext data,
        ICurrentUserService currentUserService)
        => new(
            data,
            currentUserService,
            new PageClamper(),
            Substitute.For<ILogger<NotificationService>>());

    private static CreateBookServiceModel NewCreateBookServiceModel(
        ICollection<Guid>? genres = null)
        => new()
        {
            Title = "A valid book title",
            AuthorId = null,
            Image = null,
            ShortDescription = "A valid short description",
            LongDescription = new string('l', 200),
            PublishedDate = null,
            Genres = genres ?? []
        };

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
        ICurrentUserService currentUserService,
        IAdminService? adminService = null,
        INotificationService? notificationService = null,
        IImageWriter? imageWriter = null,
        IProfileService? profileService = null)
        => new(
            data,
            imageWriter ?? Substitute.For<IImageWriter>(),
            adminService ?? Substitute.For<IAdminService>(),
            currentUserService,
            notificationService ?? Substitute.For<INotificationService>(),
            profileService ?? Substitute.For<IProfileService>(),
            new PageClamper(),
            Substitute.For<ILogger<BookService>>());

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
