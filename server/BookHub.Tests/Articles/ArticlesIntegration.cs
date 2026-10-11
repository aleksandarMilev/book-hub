namespace BookHub.Tests.Articles;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Data;
using Features.Articles.Data.Models;
using Features.Articles.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils;

using static Features.Articles.Shared.Constants.Paths;

public sealed class ArticlesIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async ValueTask InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public ValueTask DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task GetDetails_ShouldIncrementsViews()
    {
        var articleId = await this.SeedArticle(views: 0);
        var httpClient = this.httpClientFactory.CreateClient();

        var url = $"/Articles/{articleId}/";

        var firstResponse = await httpClient.GetAsync(url, TestContext.Current.CancellationToken);
        firstResponse.IsSuccessStatusCode.Should().BeTrue();

        var firstResponseBody = await firstResponse
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);

        firstResponseBody.Should().NotBeNull();
        firstResponseBody!.Id.Should().Be(articleId);
        firstResponseBody.Views.Should().Be(1);

        var secondResponse = await httpClient.GetAsync(url, TestContext.Current.CancellationToken);
        secondResponse.IsSuccessStatusCode.Should().BeTrue();

        var secondResponseBody = await secondResponse
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);

        secondResponseBody!.Views.Should().Be(2);

        using var scope = this.httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == articleId, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.Views.Should().Be(2);
    }

    [Fact]
    public async Task GetDetails_ShouldReturnNotFound_WhenNoArticleWithSuchIdInDb()
    {
        var httpClient = this.httpClientFactory.CreateClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.GetAsync($"/Articles/{nonExistingId}/", TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var httpClient = await this.httpClientFactory.CreateUserClient();

        var formData = BuildArticleForm(
            title: "A sufficiently long valid title",
            intro: "A sufficiently long valid introduction",
            content: new string('c', 200));

        var response = await httpClient.PostAsync("/Administrator/Articles", formData, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var articleId = await this.SeedArticle();
        var httpClient = await this.httpClientFactory.CreateUserClient();

        var response = await httpClient.DeleteAsync($"/Administrator/Articles/{articleId}/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }


    [Fact]
    public async Task GetDetails_ShouldNotIncrementViews_FromTheAdminController()
    {
        var artcileId = await this.SeedArticle(views: 5);
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.GetAsync($"/Administrator/Articles/{artcileId}/", TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);

        body.Should().NotBeNull();
        body!.Views.Should().Be(5);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == artcileId, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.Views.Should().Be(5);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldPersistTheArticleInTheDb_AndAlso_ShouldSetDefaultImage_WhenNoImageProvided()
    {
        var httpClient = await this.httpClientFactory.CreateAdminClient();
        var formData = BuildArticleForm(
            title: "A sufficiently long valid title",
            intro: "A sufficiently long valid introduction",
            content: new string('c', 200));

        var response = await httpClient.PostAsync("/Administrator/Articles", formData, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdArticle = await response
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);

        createdArticle.Should().NotBeNull();
        createdArticle.Id.Should().NotBeEmpty();
        createdArticle.ImagePath.Should().Be(DefaultImagePath);

        response
            .Headers
            .Location
            .ToString()
            .Should()
            .Contain($"/Articles/{createdArticle.Id}");

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == createdArticle.Id, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.ImagePath.Should().Be(DefaultImagePath);
        articleDbModel.CreatedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task Create_ShouldPersistNonDefaultImage_WhenImageProvided_AndAlso_ShouldUseImageWriter()
    {
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleFormWithImage(
            title: "A sufficiently long valid title",
            intro: "A sufficiently long valid introduction",
            content: new string('c', 200));

        var response = await httpClient.PostAsync("/Administrator/Articles", formData, TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdArticle = await response
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);

        createdArticle.Should().NotBeNull();
        createdArticle.ImagePath.Should().NotBe(DefaultImagePath);
        createdArticle.ImagePath.Should().MatchRegex(@"^/images/articles/[0-9a-f-]{36}\.png$");

        // The uploaded file is on disk, and it's the only one.
        this.httpClientFactory.ImageFiles(ImagePathPrefix).Should().Equal(createdArticle.ImagePath);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == createdArticle.Id, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.ImagePath.Should().Be(createdArticle.ImagePath);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidModelProvided()
    {
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleForm(
            title: "short",
            intro: "short",
            content: "too short");

        var response = await httpClient.PostAsync("/Administrator/Articles", formData, TestContext.Current.CancellationToken);

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
    public async Task Edit_ShouldReturnsNoContent_AndAlso_ShouldUpdateFields_WhenArticleExists()
    {
        var seedImagePath = this.httpClientFactory.SeedImageFile(ImagePathPrefix, "seed.jpg");
        var articleId = await this.SeedArticle(imagePath: seedImagePath);

        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleForm(
            title: "Edited valid title long enough",
            intro: "Edited valid introduction long enough",
            content: new string('z', 250));

        var response = await httpClient.PutAsync($"/Administrator/Articles/{articleId}/", formData, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == articleId, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.Title.Should().Be("Edited valid title long enough");
        articleDbModel.ImagePath.Should().Be(seedImagePath);
        articleDbModel.ModifiedOn.Should().NotBeNull();

        // No new image, so the current one is kept and not deleted.
        this.httpClientFactory.ImageFiles(ImagePathPrefix).Should().Equal(seedImagePath);
    }

    [Fact]
    public async Task Edit_ShouldReturnNotFoundProblem_WhenArticleDoesNotExist()
    {
        var httpClient = await this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var formData = BuildArticleForm(
            title: "Edited valid title long enough",
            intro: "Edited valid introduction long enough",
            content: new string('z', 250));

        var response = await httpClient.PutAsync($"/Administrator/Articles/{nonExistingId}/", formData, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The article was not found.");

        this.httpClientFactory.ImageFiles(ImagePathPrefix).Should().BeEmpty();
    }

    [Fact]
    public async Task Edit_ShouldUpdateImage_AndAlso_ShouldDeleteOldImage_WhenNewImageProvided()
    {
        var oldImagePath = this.httpClientFactory.SeedImageFile(ImagePathPrefix, "old.jpg");
        var articleId = await this.SeedArticle(imagePath: oldImagePath);
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleFormWithImage(
            title: "Edited valid title long enough",
            intro: "Edited valid introduction long enough",
            content: new string('z', 250));

        var response = await httpClient.PutAsync($"/Administrator/Articles/{articleId}/", formData, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);


        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == articleId, cancellationToken: TestContext.Current.CancellationToken);

        articleDbModel.ImagePath.Should().NotBe(oldImagePath);
        articleDbModel.ModifiedOn.Should().NotBeNull();

        // The new image replaced the old file on disk.
        this.httpClientFactory.ImageFiles(ImagePathPrefix).Should().Equal(articleDbModel.ImagePath);
    }

    [Fact]
    public async Task Edit_ShouldReturnBadRequest_WhenInvalidModelProvided()
    {
        var articleId = await this.SeedArticle();
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleForm(
            title: "short",
            intro: "short",
            content: "too short");

        var response = await httpClient.PutAsync($"/Administrator/Articles/{articleId}/", formData, TestContext.Current.CancellationToken);

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
    public async Task Delete_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenArticleExists()
    {
        var articleId = await this.SeedArticle();
        var httpClient = await this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.DeleteAsync($"/Administrator/Articles/{articleId}/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articlesCount = await data
            .Articles
            .CountAsync(a => a.Id == articleId, cancellationToken: TestContext.Current.CancellationToken);

        articlesCount.Should().Be(0);

        var deletedArticle = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == articleId, cancellationToken: TestContext.Current.CancellationToken);

        deletedArticle.IsDeleted.Should().BeTrue();
        deletedArticle.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundProblem_WhenArticleDoesNotExist()
    {
        var httpClient = await this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync($"/Administrator/Articles/{nonExistingId}/", TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(
            HttpStatusCode.NotFound,
            "The article was not found.");
    }

    private static MultipartFormDataContent BuildArticleForm(
       string title,
       string intro,
       string content)
    {
        return new()
        {
            { new StringContent(title), "Title" },
            { new StringContent(intro), "Introduction" },
            { new StringContent(content), "Content" }
        };
    }

    private static MultipartFormDataContent BuildArticleFormWithImage(
        string title,
        string intro,
        string content)
    {
        return BuildArticleForm(title, intro, content)
            .WithFile(Images.Png, "test.png", "image/png");
    }


    private async Task<Guid> SeedArticle(
        string imagePath = "/images/articles/seed.jpg",
        int views = 0)
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var articleDbModel = new ArticleDbModel
        {
            Id = Guid.NewGuid(),
            Title = "User endpoint article title",
            Introduction = "User endpoint introduction",
            Content = new string('a', 200),
            ImagePath = imagePath,
            Views = views
        };

        data.Articles.Add(articleDbModel);
        await data.SaveChangesAsync();

        return articleDbModel.Id;
    }
}
