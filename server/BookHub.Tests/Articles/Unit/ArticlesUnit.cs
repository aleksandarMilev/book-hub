namespace BookHub.Tests.Articles.Unit;

using BookHub.Data;
using Features.Articles.Data.Models;
using Features.Articles.Service;
using Features.Articles.Service.Models;
using FluentAssertions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

using static Features.Articles.Shared.Constants.Paths;

public sealed class ArticlesUnit
{
    [Fact]
    public async Task Details_ShouldIncrementViews_AndAlso_ShouldReturnServiceModel_WhenNotEditMode()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();

        var article = NewArticle(views: 0);

        data.Articles.Add(article);
        await data.SaveChangesAsync();

        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var serviceModelResult = await service.Details(
            article.Id,
            isEditMode: false);

        data.ChangeTracker.Clear();

        serviceModelResult.Should().NotBeNull();
        serviceModelResult.Id.Should().Be(article.Id);
        serviceModelResult.Views.Should().Be(1);

        var dbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == article.Id);

        dbModel.Views.Should().Be(1);
    }

    [Fact]
    public async Task Details_ShouldNotIncrementViews_WhenEditMode()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();

        var article = NewArticle(views: 7);

        data.Articles.Add(article);
        await data.SaveChangesAsync();

        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var serviceModelResult = await service.Details(
            article.Id,
            isEditMode: true);

        serviceModelResult.Should().NotBeNull();
        serviceModelResult.Views.Should().Be(7);

        var dbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == article.Id);

        dbModel.Views.Should().Be(7);
    }

    [Fact]
    public async Task Details_ShouldReturnNull_WhenArtcileWithSuchIdNotInTheDb()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();

        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var result = await service.Details(
            Guid.NewGuid(),
            isEditMode: false);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Create_ShouldSetDefaultImagePath_AndAlso_ShouldPersistArticleInDb_AndAlso_ShouldReturnServiceModel_WhenImageNotProvided()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

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

        var logger = Substitute.For<ILogger<ArticleService>>();
        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var serviceModel = new CreateArticleServiceModel
        {
            Title = "A valid article title",
            Introduction = "A valid article introduction long enough",
            Content = new string('c', 200),
            Image = null
        };

        var createdArticle = await service.Create(serviceModel);

        createdArticle.Id.Should().NotBeEmpty();
        createdArticle.ImagePath.Should().Be(DefaultImagePath);
        createdArticle.Title.Should().Be(serviceModel.Title);

        var dbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == createdArticle.Id);

        dbModel.ImagePath.Should().Be(DefaultImagePath);
        dbModel.CreatedOn.Should().NotBe(default);

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
    public async Task Edit_ShouldReturnNotFoundResult_AndAlso_ShouldNotWriteImage_WhenArtcileWithSuchIdNotInTheDb()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();

        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var nonExistingId = Guid.NewGuid();
        var serviceModel = new CreateArticleServiceModel
        {
            Title = "A valid article title",
            Introduction = "A valid article introduction long enough",
            Content = new string('c', 200),
            Image = null
        };

        var result = await service.Edit(
            nonExistingId,
            serviceModel);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"ArticleDbModel with Id: {nonExistingId} was not found!");

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
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

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
                dbModel.ImagePath = "/images/articles/new.jpg";
            });

        imageWriter
            .Delete(
                resourceName: Arg.Any<string>(),
                imagePath: Arg.Any<string?>(),
                defaultImagePath: Arg.Any<string?>())
            .Returns(true);

        var logger = Substitute.For<ILogger<ArticleService>>();
        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var article = NewArticle(imagePath: "/images/articles/old.jpg");

        data.Articles.Add(article);
        await data.SaveChangesAsync();

        var dummyFile = new FormFile(
            baseStream: new MemoryStream([1, 2, 3]),
            baseStreamOffset: 0,
            length: 3,
            name: "Image",
            fileName: "test.jpg");

        var serviceModel = new CreateArticleServiceModel
        {
            Title = "Updated title is valid",
            Introduction = "Updated introduction is valid",
            Content = new string('u', 200),
            Image = dummyFile
        };

        var result = await service.Edit(
            article.Id,
            serviceModel);

        result.Succeeded.Should().BeTrue();

        var dbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == article.Id);

        dbModel.Title.Should().Be(serviceModel.Title);
        dbModel.ImagePath.Should().Be("/images/articles/new.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();

        imageWriter
            .Received(1)
            .Delete(
                ImagePathPrefix,
                "/images/articles/old.jpg",
                DefaultImagePath);
    }

    [Fact]
    public async Task Edit_ShouldNotDeleteOldImage_WhenNoNewImageProvided()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();
        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var article = NewArticle(imagePath: "/images/articles/old.jpg");

        data.Articles.Add(article);
        await data.SaveChangesAsync();

        var serviceModel = new CreateArticleServiceModel
        {
            Title = "Updated title is valid",
            Introduction = "Updated introduction is valid",
            Content = new string('u', 200),
            Image = null
        };

        var result = await service.Edit(
            article.Id,
            serviceModel);

        result.Succeeded.Should().BeTrue();

        imageWriter
            .DidNotReceive()
            .Delete(
                resourceName: Arg.Any<string>(),
                imagePath: Arg.Any<string?>(),
                defaultImagePath: Arg.Any<string?>());
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteArticle_AndAlso_ShouldFilterItOut()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();
        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var article = NewArticle();

        data.Articles.Add(article);
        await data.SaveChangesAsync();

        var result = await service.Delete(article.Id);

        result.Succeeded.Should().BeTrue();

        var articlesCount = await data
            .Articles
            .CountAsync(a => a.Id == article.Id);

        articlesCount.Should().Be(0);

        var deletedDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == article.Id);

        deletedDbModel.IsDeleted.Should().BeTrue();
        deletedDbModel.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundResult_WhenArtcileWithSuchIdNotInTheDb()
    {
        var (data, connection) = await CreateSqliteDb();
        await using var _ = connection;

        var imageWriter = Substitute.For<IImageWriter>();
        var logger = Substitute.For<ILogger<ArticleService>>();
        var service = new ArticleService(
            data,
            imageWriter,
            logger);

        var nonExistingId = Guid.NewGuid();
        var result = await service.Delete(nonExistingId);

        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be($"ArticleDbModel with Id: {nonExistingId} was not found!");
    }

    private static async Task<(BookHubDbContext Data, SqliteConnection Connection)> CreateSqliteDb(
       string username = "shano")
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseSqlite(connection)
            .Options;

        var currentUsername = Substitute.For<ICurrentUserService>();
        currentUsername
            .GetUsername()
            .Returns(username);

        var data = new BookHubDbContext(options, currentUsername);
        await data.Database.EnsureCreatedAsync();

        return (data, connection);
    }

    private static ArticleDbModel NewArticle(
        Guid? articleId = null,
        int views = 0,
        string imagePath = "/images/articles/old.jpg")
        => new()
        {
            Id = articleId ?? Guid.NewGuid(),
            Title = "A valid article title",
            Introduction = "A valid article introduction long enough",
            Content = new string('c', 200),
            ImagePath = imagePath,
            Views = views
        };
}
