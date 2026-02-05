namespace BookHub.Tests.Authors.Unit;

using Areas.Admin.Service;
using Data;
using Features.Authors.Data.Models;
using Features.Authors.Service;
using Features.Authors.Service.Models;
using Features.Authors.Shared;
using Features.Notifications.Service;
using Features.UserProfile.Service;
using FluentAssertions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

using static Features.Authors.Shared.Constants.Paths;

public sealed class AuthorsUnit
{
    [Fact]
    public async Task Names_ShouldReturnAllAuthorsAsNamesServiceModels()
    {
        var (data, connection, _) = await CreateSqliteDb();
        await using var _ = connection;

        var userService = Substitute.For<ICurrentUserService>();
        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var author1 = NewAuthor(name: "A1", averageRating: 3.2, isApproved: true);
        var author2 = NewAuthor(name: "A2", averageRating: 4.1, isApproved: true);

        data.Authors.AddRange(author1, author2);
        await data.SaveChangesAsync();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var result = (await service.Names()).ToList();

        result.Should().HaveCount(2);
        result.Select(a => a.Id).Should().Contain([author1.Id, author2.Id]);
        result.Select(a => a.Name).Should().Contain(["A1", "A2"]);
    }

    [Fact]
    public async Task TopThree_ShouldReturnThreeAuthorsOrderedByAverageRatingDesc()
    {
        var (data, connection, _) = await CreateSqliteDb();
        await using var _ = connection;

        var userService = Substitute.For<ICurrentUserService>();
        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var author1  = NewAuthor(name: "A1", averageRating: 1.1, isApproved: true);
        var author2 = NewAuthor(name: "A2", averageRating: 4.6, isApproved: true);
        var author3 = NewAuthor(name: "A3", averageRating: 3.8, isApproved: true);
        var author4 = NewAuthor(name: "A4", averageRating: 5.0, isApproved: true);

        data.Authors.AddRange(
            author1,
            author2,
            author3,
            author4);

        await data.SaveChangesAsync();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var result = (await service.TopThree()).ToList();

        result.Should().HaveCount(3);
        result[0].Id.Should().Be(author4.Id);
        result[1].Id.Should().Be(author2.Id);
        result[2].Id.Should().Be(author3.Id);
    }

    [Fact]
    public async Task Details_ShouldReturnNull_WhenAuthorWithSuchIdNotInTheDb()
    {
        var (data, connection, _) = await CreateSqliteDb();
        await using var _ = connection;

        var userService = Substitute.For<ICurrentUserService>();
        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var result = await service.Details(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task Create_ShouldSetDefaultImagePath_AndAlso_ShouldPersistAuthorInDb_AndAlso_ShouldSetCreatorId_AndAlso_ShouldNotApprove_WhenNonAdmin()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "user-1",
            username: "user",
            isAdmin: false);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("user-1");
        userService.IsAdmin().Returns(false);

        var adminService = Substitute.For<IAdminService>();
        adminService.GetId().Returns("admin-1");

        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();

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

        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var serviceModel = new CreateAuthorServiceModel
        {
            Name = "Valid author name",
            Biography = new string('b', 120),
            PenName = "Pen",
            Nationality = Nationality.Bulgaria,
            Gender = Gender.Male,
            BornAt = null,
            DiedAt = null,
            Image = null
        };

        var result = await service.Create(serviceModel);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.ImagePath.Should().Be(DefaultImagePath);

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == result.Data.Id);

        dbModel.CreatorId.Should().Be("user-1");
        dbModel.ImagePath.Should().Be(DefaultImagePath);
        dbModel.IsApproved.Should().BeFalse();
        dbModel.CreatedOn.Should().NotBe(default);

        await notificationService
            .Received(1)
            .CreateOnAuthorCreation(
                dbModel.Id,
                dbModel.Name,
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
    public async Task Create_ShouldReturnErrorResult_WhenGenderEnumIsInvalid()
    {
        var (data, connection, currentUser) = await CreateSqliteDb();
        await using var _ = connection;

        var userService = currentUser;
        userService.IsAdmin().Returns(false);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var serviceModel = new CreateAuthorServiceModel
        {
            Name = "Valid author name",
            Biography = new string('b', 120),
            PenName = null,
            Nationality = Nationality.Bulgaria,
            Gender = (Gender)999_999_999,
            BornAt = null,
            DiedAt = null,
            Image = null
        };

        var result = await service.Create(serviceModel);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"{serviceModel.Gender} is not valid Gender enumeartion!");
    }

    [Fact]
    public async Task Edit_ShouldReturnNotFoundResult_AndAlso_ShouldNotWriteImage_WhenAuthorWithSuchIdNotInTheDb()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("admin-1");
        userService.IsAdmin().Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var nonExistingId = Guid.NewGuid();

        var serviceModel = new CreateAuthorServiceModel
        {
            Name = "Updated author name",
            Biography = new string('b', 180),
            PenName = null,
            Nationality = Nationality.France,
            Gender = Gender.Other,
            BornAt = null,
            DiedAt = null,
            Image = null
        };

        var result = await service.Edit(
            nonExistingId,
            serviceModel);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"AuthorDbModel with Id: {nonExistingId} was not found!");

        await imageWriter
            .DidNotReceiveWithAnyArgs()
            .Write(
                resourceName: default!,
                dbModel: default!,
                serviceModel: default!,
                defaultImagePath: default,
                cancellationToken: default);
    }

    [Fact]
    public async Task Edit_ShouldChangeImagePath_AndAlso_ShouldDeletesOldImage_WhenNewImageProvided()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("admin-1");
        userService.IsAdmin().Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();

        var imageWriter = Substitute.For<IImageWriter>();
        imageWriter
            .When(writter => writter.Write(
                Arg.Any<string>(),
                Arg.Any<IImageDdModel>(),
                Arg.Any<IImageServiceModel>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                var dbModel = (IImageDdModel)callInfo[1];
                dbModel.ImagePath = "/images/authors/new.jpg";
            });

        imageWriter
            .Delete(
                resourceName: Arg.Any<string>(),
                imagePath: Arg.Any<string?>(),
                defaultImagePath: Arg.Any<string?>())
            .Returns(true);

        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var author = NewAuthor(
            creatorId: "admin-1",
            imagePath: "/images/authors/old.jpg",
            isApproved: true);

        data.Authors.Add(author);
        await data.SaveChangesAsync();

        var dummyFile = new FormFile(
            baseStream: new MemoryStream([1, 2, 3]),
            baseStreamOffset: 0,
            length: 3,
            name: "Image",
            fileName: "test.jpg");

        var serviceModel = new CreateAuthorServiceModel
        {
            Name = "Updated name",
            Biography = new string('b', 180),
            PenName = "Updated pen",
            Nationality = Nationality.Bulgaria,
            Gender = Gender.Male,
            BornAt = null,
            DiedAt = null,
            Image = dummyFile
        };

        var result = await service.Edit(
            author.Id,
            serviceModel);

        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == author.Id);

        dbModel.Name.Should().Be(serviceModel.Name);
        dbModel.ImagePath.Should().Be("/images/authors/new.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();

        imageWriter
            .Received(1)
            .Delete(
                nameof(AuthorDbModel),
                "/images/authors/old.jpg",
                DefaultImagePath);
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteAuthor_AndAlso_ShouldFilterItOut()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("admin-1");
        userService.IsAdmin().Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var author = NewAuthor(
            creatorId: "admin-1",
            isApproved: true);

        data.Authors.Add(author);
        await data.SaveChangesAsync();

        var result = await service.Delete(author.Id);

        result.Succeeded.Should().BeTrue();

        var count = await data
            .Authors
            .CountAsync(a => a.Id == author.Id);

        count.Should().Be(0);

        var deleted = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == author.Id);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Approve_ShouldReturnUnauthorizedResult_WhenNotAdmin()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "user-1",
            username: "user",
            isAdmin: false);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("user-1");
        userService.IsAdmin().Returns(false);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var authorId = Guid.NewGuid();

        var result = await service.Approve(authorId);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"User with Id: user-1 is not authorized to access AuthorDbModel with Id: {authorId}!");
    }

    [Fact]
    public async Task Approve_ShouldSetIsApprovedTrue_AndAlso_ShouldNotifyCreator_AndAlso_ShouldIncrementCreatedAuthorsCount()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("admin-1");
        userService.IsAdmin().Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var author = NewAuthor(
            creatorId: "user-1",
            isApproved: false);

        data.Authors.Add(author);
        await data.SaveChangesAsync();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var result = await service.Approve(author.Id);

        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == author.Id);

        dbModel.IsApproved.Should().BeTrue();

        await notificationService
            .Received(1)
            .CreateOnAuthorApproved(
                author.Id,
                author.Name,
                "user-1",
                Arg.Any<CancellationToken>());

        await profileService
            .Received(1)
            .IncrementCreatedAuthorsCount(
                "user-1",
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Reject_ShouldSoftDeleteAuthor_AndAlso_ShouldNotifyCreator()
    {
        var (data, connection, currentUser) = await CreateSqliteDb(
            userId: "admin-1",
            username: "admin",
            isAdmin: true);

        await using var _ = connection;

        var userService = currentUser;
        userService.GetId().Returns("admin-1");
        userService.IsAdmin().Returns(true);

        var adminService = Substitute.For<IAdminService>();
        var profileService = Substitute.For<IProfileService>();
        var notificationService = Substitute.For<INotificationService>();
        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<AuthorService>>();

        var author = NewAuthor(
            creatorId: "user-1",
            isApproved: false);

        data.Authors.Add(author);
        await data.SaveChangesAsync();

        var service = new AuthorService(
            data,
            userService,
            adminService,
            profileService,
            notificationService,
            imageWriter,
            logger);

        var result = await service.Reject(author.Id);

        result.Succeeded.Should().BeTrue();

        var count = await data
            .Authors
            .CountAsync(a => a.Id == author.Id);

        count.Should().Be(0);

        var deleted = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == author.Id);

        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedOn.Should().NotBeNull();

        await notificationService
            .Received(1)
            .CreateOnAuthorRejected(
                author.Id,
                author.Name,
                "user-1",
                Arg.Any<CancellationToken>());
    }

    private static async Task<(
        BookHubDbContext Data,
        SqliteConnection Connection,
        ICurrentUserService CurrentUser)>
        CreateSqliteDb(
            string userId = "user-1",
            string username = "user",
            bool isAdmin = false)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseSqlite(connection)
            .Options;

        var currentUser = Substitute.For<ICurrentUserService>();
        currentUser.GetUsername().Returns(username);
        currentUser.GetId().Returns(userId);
        currentUser.IsAdmin().Returns(isAdmin);

        var data = new BookHubDbContext(options, currentUser);
        await data.Database.EnsureCreatedAsync();

        return (data, connection, currentUser);
    }

    private static AuthorDbModel NewAuthor(
        Guid? authorId = null,
        string name = "A valid author name",
        string biography = null!,
        string? penName = "Pen",
        string imagePath = "/images/authors/old.jpg",
        string? creatorId = "user-1",
        double averageRating = 0,
        bool isApproved = false)
        => new()
        {
            Id = authorId ?? Guid.NewGuid(),
            Name = name,
            Biography = biography ?? new string('b', 120),
            PenName = penName,
            ImagePath = imagePath,
            Nationality = Nationality.Bulgaria,
            Gender = Gender.Other,
            BornAt = null,
            DiedAt = null,
            CreatorId = creatorId,
            AverageRating = averageRating,
            IsApproved = isApproved
        };
}
