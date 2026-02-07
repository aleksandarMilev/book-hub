namespace BookHub.Tests.Authors.Integration;

using BookHub.Data;
using BookHub.Features.Authors.Data.Models;
using BookHub.Features.Authors.Service.Models;
using BookHub.Features.Authors.Shared;
using BookHub.Features.Identity.Data.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using static BookHub.Features.Authors.Shared.Constants.Paths;

public sealed class AuthorsIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory httpClientFactory = new();

    public async Task InitializeAsync()
    {
        await this
            .httpClientFactory
            .ResetDatabase();

        await this.SeedUserAsync("test-user", "user");
        await this.SeedUserAsync("test-admin-id", "admin");
    }

    public Task DisposeAsync()
    {
        this.httpClientFactory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldPersistTheAuthorInTheDb_AndAlso_ShouldSetDefaultImage_WhenNoImageProvided()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildAuthorForm(
            name: "A valid author name",
            biography: new string('b', 120),
            penName: "Valid pen name",
            nationality: Nationality.Bulgaria,
            gender: Gender.Male,
            bornAt: new DateTime(1950, 1, 1),
            diedAt: null);

        var response = await httpClient.PostAsync("/Authors", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdAuthor = await response
            .Content
            .ReadFromJsonAsync<AuthorDetailsServiceModel>();

        createdAuthor.Should().NotBeNull();
        createdAuthor!.Id.Should().NotBeEmpty();
        createdAuthor.ImagePath.Should().Be(DefaultImagePath);

        response
            .Headers
            .Location
            .ToString()
            .Should()
            .Contain($"/Authors/{createdAuthor.Id}");

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == createdAuthor.Id);

        dbModel.ImagePath.Should().Be(DefaultImagePath);
        dbModel.IsApproved.Should().BeFalse();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtRoute_AndAlso_ShouldAutoApprove_WhenAdminCreates()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();

        var formData = BuildAuthorForm(
            name: "Admin created author",
            biography: new string('b', 140),
            penName: null,
            nationality: Nationality.UnitedStates,
            gender: Gender.Female,
            bornAt: null,
            diedAt: null);

        var response = await httpClient.PostAsync("/Authors", formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAuthor = await response
            .Content
            .ReadFromJsonAsync<AuthorDetailsServiceModel>();

        createdAuthor.Should().NotBeNull();
        createdAuthor!.Id.Should().NotBeEmpty();

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var dbModel = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == createdAuthor.Id);

        dbModel.IsApproved.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidModelProvided()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildAuthorForm(
            name: "a",
            biography: "short",
            penName: null,
            nationality: (Nationality)999_999_999,
            gender: (Gender)999_999_999,
            bornAt: null,
            diedAt: null);

        var response = await httpClient.PostAsync("/Authors", formData);

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
    public async Task Edit_ShouldReturnsNoContent_AndAlso_ShouldUpdateFields_WhenAuthorExists()
    {
        var authorId = await this.SeedAuthor(
            isApproved: true,
            imagePath: "/images/authors/seed.jpg");

        var httpClient = this.httpClientFactory.CreateUserClient();

        var formData = BuildAuthorForm(
            name: "Edited author name",
            biography: new string('x', 200),
            penName: "Edited pen name",
            nationality: Nationality.France,
            gender: Gender.Other,
            bornAt: new DateTime(1940, 2, 2),
            diedAt: new DateTime(2020, 3, 3));

        var response = await httpClient.PutAsync(
            $"/Authors/{authorId}/",
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
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == authorId);

        dbModel.Name.Should().Be("Edited author name");
        dbModel.ImagePath.Should().Be("/images/authors/seed.jpg");
        dbModel.ModifiedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Edit_ShouldReturnBadRequestWithErrorMessage_WhenAuthorDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();
        var nonExistingId = Guid.NewGuid();

        var formData = BuildAuthorForm(
            name: "Edited author name",
            biography: new string('x', 200),
            penName: "Edited pen name",
            nationality: Nationality.France,
            gender: Gender.Other,
            bornAt: null,
            diedAt: null);

        var response = await httpClient.PutAsync(
            $"/Authors/{nonExistingId}/",
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
            .Be($"AuthorDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenAuthorExists()
    {
        var authorId = await this.SeedAuthor(
            isApproved: true);

        var httpClient = this.httpClientFactory.CreateUserClient();

        var response = await httpClient.DeleteAsync(
            $"/Authors/{authorId}/");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var authorsCount = await data
            .Authors
            .CountAsync(a => a.Id == authorId);

        authorsCount.Should().Be(0);

        var deletedAuthor = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == authorId);

        deletedAuthor.IsDeleted.Should().BeTrue();
        deletedAuthor.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequestWithErrorMessage_WhenAuthorDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateUserClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.DeleteAsync(
            $"/Authors/{nonExistingId}/");

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
            .Be($"AuthorDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Approve_ShouldReturnNoContent_AndAlso_ShouldSetIsApprovedTrue_WhenAuthorExists()
    {
        var authorId = await this.SeedAuthor(
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Authors/{authorId}/approve/",
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
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == authorId);

        dbModel.IsApproved.Should().BeTrue();
    }

    [Fact]
    public async Task Approve_ShouldReturnBadRequestWithErrorMessage_WhenAuthorDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Authors/{nonExistingId}/approve/",
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
            .Be($"AuthorDbModel with Id: {nonExistingId} was not found!");
    }

    [Fact]
    public async Task Reject_ShouldReturnNoContent_AndAlso_ShouldSoftDelete_WhenAuthorExists()
    {
        var authorId = await this.SeedAuthor(
            isApproved: false);

        var httpClient = this.httpClientFactory.CreateAdminClient();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Authors/{authorId}/reject/",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var authorsCount = await data
            .Authors
            .CountAsync(a => a.Id == authorId);

        authorsCount.Should().Be(0);

        var deletedAuthor = await data
            .Authors
            .IgnoreQueryFilters()
            .SingleAsync(a => a.Id == authorId);

        deletedAuthor.IsDeleted.Should().BeTrue();
        deletedAuthor.DeletedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task Reject_ShouldReturnBadRequestWithErrorMessage_WhenAuthorDoesNotExist()
    {
        var httpClient = this.httpClientFactory.CreateAdminClient();
        var nonExistingId = Guid.NewGuid();

        var response = await httpClient.PatchAsync(
            $"/Administrator/Authors/{nonExistingId}/reject/",
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
            .Be($"AuthorDbModel with Id: {nonExistingId} was not found!");
    }

    private static MultipartFormDataContent BuildAuthorForm(
        string name,
        string biography,
        string? penName,
        Nationality nationality,
        Gender gender,
        DateTime? bornAt,
        DateTime? diedAt)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(name), "Name" },
            { new StringContent(biography), "Biography" },
            { new StringContent(((int)nationality).ToString()), "Nationality" },
            { new StringContent(((int)gender).ToString()), "Gender" }
        };

        if (!string.IsNullOrWhiteSpace(penName))
        {
            form.Add(new StringContent(penName), "PenName");
        }

        if (bornAt is not null)
        {
            form.Add(new StringContent(bornAt.Value.ToString("O")), "BornAt");
        }

        if (diedAt is not null)
        {
            form.Add(new StringContent(diedAt.Value.ToString("O")), "DiedAt");
        }

        return form;
    }

    private async Task<Guid> SeedAuthor(
        bool isApproved,
        string imagePath = "/images/authors/seed.jpg")
    {
        using var scope = this
            .httpClientFactory
            .Services
            .CreateScope();

        var data = scope
            .ServiceProvider
            .GetRequiredService<BookHubDbContext>();

        var authorDbModel = new AuthorDbModel
        {
            Id = Guid.NewGuid(),
            Name = "Seed author",
            Biography = new string('b', 100),
            PenName = "Seed pen name",
            Gender = Gender.Other,
            Nationality = Nationality.Bulgaria,
            BornAt = new DateTime(1900, 1, 1),
            DiedAt = null,
            ImagePath = imagePath,
            IsApproved = isApproved,
            CreatorId = "test-user"
        };

        data.Authors.Add(authorDbModel);
        await data.SaveChangesAsync();

        return authorDbModel.Id;
    }

    private async Task SeedUserAsync(
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
            .Set<UserDbModel>()
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

        data.Set<UserDbModel>().Add(user);
        await data.SaveChangesAsync();
    }
}
