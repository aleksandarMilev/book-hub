namespace BookHub.Tests.Articles.Integration;

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BookHub.Data;
using Features.Articles.Data.Models;
using Features.Articles.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using static Features.Articles.Shared.Constants.Paths;

public sealed class ArticlesIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async Task InitializeAsync()
        => await this.httpClientFactory.ResetDatabase();

    public Task DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetDetails_ShouldIncrementsViews()
    {
        var articleId = await SeedArticle(views: 0);
        var httpClient = this.httpClientFactory.CreateClient();

        var url = $"/Articles/{articleId}/";

        var firstResponse = await httpClient.GetAsync(url);
        firstResponse.IsSuccessStatusCode.Should().BeTrue();

        var firstResponseBody = await firstResponse
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>();

        firstResponseBody.Should().NotBeNull();
        firstResponseBody!.Id.Should().Be(articleId);
        firstResponseBody.Views.Should().Be(1);

        var secondResponse = await httpClient.GetAsync(url);
        secondResponse.IsSuccessStatusCode.Should().BeTrue();

        var secondResponseBody = await secondResponse
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>();

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
            .SingleAsync(a => a.Id == articleId);

        articleDbModel.Views.Should().Be(2);
    }

    [Fact]
    public async Task GetDetails_ShouldReturnNotFound_WhenNoArticleWithSuchIdInDb()
    {
        var httpClient = this.httpClientFactory.CreateClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.GetAsync($"/Articles/{nonExistingId}/");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetDetails_ShouldNotIncrementViews_FromTheAdminController()
    {
        var artcileId = await SeedArticle(views: 5);
        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.GetAsync($"/Administrator/Articles/{artcileId}/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>();

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
            .SingleAsync(a => a.Id == artcileId);

        articleDbModel.Views.Should().Be(5);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldPersistTheArticleInTheDb_AndAlso_ShouldSetDefaultImage_WhenNoImageProvided()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var formData = BuildArticleForm(
            title: "A sufficiently long valid title",
            intro: "A sufficiently long valid introduction",
            content: new string('c', 200));

        var response = await httpClient.PostAsync("/Administrator/Articles", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdArticle = await response
            .Content
            .ReadFromJsonAsync<ArticleDetailsServiceModel>();

        createdArticle.Should().NotBeNull();
        createdArticle!.Id.Should().NotBeEmpty();
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
            .SingleAsync(a => a.Id == createdArticle.Id);

        articleDbModel.ImagePath.Should().Be(DefaultImagePath);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidModelProvided()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();

        var formData = BuildArticleForm(
            title: "short",
            intro: "short",
            content: "too short");

        var response = await httpClient.PostAsync(
            "/Administrator/Articles",
            formData);

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
        var articleId = await SeedArticle();

        var httpClient = this.httpClientFactory.CreateAdminClient();
        var formData = BuildArticleForm(
            title: "Edited valid title long enough",
            intro: "Edited valid introduction long enough",
            content: new string('z', 250));

        var response = await httpClient.PutAsync(
            $"/Administrator/Articles/{articleId}/",
            formData);

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
            .SingleAsync(a => a.Id == articleId);

        articleDbModel.Title.Should().Be("Edited valid title long enough");
        articleDbModel.ImagePath.Should().Be("/images/articles/seed.jpg");
        articleDbModel.ModifiedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Edit_ShouldReturnBadRequestWithErrorMessage_WhenArticleDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var formData = BuildArticleForm(
            title: "Edited valid title long enough",
            intro: "Edited valid introduction long enough",
            content: new string('z', 250));

        var response = await httpClient.PutAsync(
            $"/Administrator/Articles/{nonExistingId}/",
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
           .Be($"ArticleDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenArticleExists()
    {
        var articleId = await SeedArticle();
        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.DeleteAsync(
            $"/Administrator/Articles/{articleId}/");

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
            .CountAsync(a => a.Id == articleId);

        articlesCount.Should().Be(0);

        var deletedArticle = await data
            .Articles
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == articleId);

        deletedArticle.IsDeleted.Should().BeTrue();
        deletedArticle.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequestWithErrorMessage_WhenArticleDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync(
            $"/Administrator/Articles/{nonExistingId}/");

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
            .Be($"ArticleDbModel with Id: {nonExistingId} was not found!");
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
