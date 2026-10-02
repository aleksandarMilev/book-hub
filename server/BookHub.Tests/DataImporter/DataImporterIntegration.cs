namespace BookHub.Tests.DataImporter;

using System.Net;
using System.Net.Http.Json;
using Features.DataImporter.Service.Models;
using Features.Search.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Seed;

// The importer runs against the real seed files in Features/DataImporter/Data.
public sealed class DataImporterIntegration : IAsyncLifetime
{
    private readonly BookHubWebApplicationFactory factory = new();

    public async Task InitializeAsync()
    {
        await this.factory.ResetDatabase();
        await this.factory.WithData(data => data.SeedUser("test-admin-id", "admin"));
    }

    public async Task DisposeAsync()
        => await this.factory.DisposeAsync();

    [Fact]
    public async Task Genres_ShouldSkipTheMigrationSeededOtherGenre_InsteadOfFailing()
    {
        var response = await this.factory
            .CreateAdminClient()
            .PostAsync("/Administrator/DataImporter/genres/", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<DataImportPartResult>();
        result!.SkippedExisting.Should().Be(1);
        result.Inserted.Should().Be(result.TotalInFile - 1);

        var otherGenres = await this.factory.WithData(data => data
            .Genres
            .CountAsync(g => g.Id == TestSeeder.OtherGenreId));

        otherGenres.Should().Be(1);
    }

    [Fact]
    public async Task All_ShouldImportEverything_AndAlso_ShouldMakeTheAuthorsSearchableInBulgarian()
    {
        var admin = this.factory.CreateAdminClient();

        var response = await admin.PostAsync("/Administrator/DataImporter/all/", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<DataImportResult>();
        result!.Books.Inserted.Should().BeGreaterThan(0);
        result.Authors.Inserted.Should().BeGreaterThan(0);
        result.Articles.Inserted.Should().BeGreaterThan(0);
        result.BooksGenres.Inserted.Should().BeGreaterThan(0);
        result.Genres.SkippedExisting.Should().Be(1);

        // authors.json has the pen name "Щастливеца" (Aleko Konstantinov).
        var search = await admin.GetFromJsonAsync<SearchPage>(
            $"/Search/authors/?searchTerm={Uri.EscapeDataString("щаст")}");

        search!.Items.Should().ContainSingle(a => a.PenName == "Щастливеца");
    }

    private sealed record SearchPage(List<SearchAuthorServiceModel> Items, int TotalItems);
}
