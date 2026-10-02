namespace BookHub.Tests.Search;

using System.Net;
using System.Net.Http.Json;
using Data;
using Features.Articles.Data.Models;
using Features.Search.Service.Models;
using FluentAssertions;
using Shared.Seed;

// Full-text search on PostgreSQL (generated tsvector columns, 'simple' config, prefix terms),
// exercised through the real HTTP endpoints.
public sealed class SearchIntegration : IAsyncLifetime
{
    private const string UserId = "search-user";

    private readonly BookHubWebApplicationFactory factory = new();

    private HttpClient client = null!;

    public async Task InitializeAsync()
    {
        await this.factory.ResetDatabase();
        await this.factory.WithData(data => data.SeedUser(UserId, "searcher"));

        this.client = this.factory.CreateUserClient(UserId, "searcher");
    }

    public async Task DisposeAsync()
        => await this.factory.DisposeAsync();

    [Theory]
    [InlineData("harr")]
    [InlineData("HARR")]
    [InlineData("Harry")]
    public async Task Books_ShouldMatchEnglishPrefix_CaseInsensitively(string term)
    {
        var harry = await this.Seed(d => d.SeedBook("Harry Potter and the Philosopher's Stone"));
        await this.Seed(d => d.SeedBook("The Hobbit"));
        await this.Seed(d => d.SeedBook("Хари Потър и философският камък"));

        var page = await this.Search<SearchBookServiceModel>("books", term);

        page.Items.Select(b => b.Id).Should().Equal(harry.Id);
        page.TotalItems.Should().Be(1);
    }

    [Theory]
    [InlineData("хар")]
    [InlineData("ХАР")]
    [InlineData("Хари пот")]
    public async Task Books_ShouldMatchBulgarianPrefix_CaseInsensitively(string term)
    {
        await this.Seed(d => d.SeedBook("Harry Potter and the Philosopher's Stone"));
        var bulgarian = await this.Seed(d => d.SeedBook("Хари Потър и философският камък"));

        var page = await this.Search<SearchBookServiceModel>("books", term);

        page.Items.Select(b => b.Id).Should().Equal(bulgarian.Id);
    }

    [Fact]
    public async Task Books_ShouldMatchShortDescription_AndAlso_ShouldRequireEveryTerm()
    {
        var dragon = await this.Seed(d => d.SeedBook(
            "The Hobbit",
            shortDescription: "A reluctant hobbit and a dragon"));

        await this.Seed(d => d.SeedBook(
            "Dragon Rider",
            shortDescription: "A young dragon looks for the rim of heaven"));

        var bothTerms = await this.Search<SearchBookServiceModel>("books", "hobb drag");
        var noBook = await this.Search<SearchBookServiceModel>("books", "hobb heaven");

        bothTerms.Items.Select(b => b.Id).Should().Equal(dragon.Id);
        noBook.Items.Should().BeEmpty();
        noBook.TotalItems.Should().Be(0);
    }

    [Fact]
    public async Task Books_ShouldNotReturnUnapprovedOrDeletedBooks()
    {
        var approved = await this.Seed(d => d.SeedBook("Dune"));
        await this.Seed(d => d.SeedBook("Dune Messiah", isApproved: false));
        await this.Seed(async d =>
        {
            var deleted = await d.SeedBook("Dune Children");
            d.Books.Remove(deleted);
            await d.SaveChangesAsync();
            return deleted;
        });

        var page = await this.Search<SearchBookServiceModel>("books", "dune");

        page.Items.Select(b => b.Id).Should().Equal(approved.Id);
    }

    [Fact]
    public async Task Books_ShouldPaginate_AndAlso_ShouldReportTheTotalOfAllMatches()
    {
        await this.Seed(d => d.SeedBook("Witcher one", averageRating: 3));
        await this.Seed(d => d.SeedBook("Witcher two", averageRating: 5));
        await this.Seed(d => d.SeedBook("Witcher three", averageRating: 4));
        await this.Seed(d => d.SeedBook("Unrelated"));

        var first = await this.Search<SearchBookServiceModel>("books", "witch", page: 1, pageSize: 2);
        var second = await this.Search<SearchBookServiceModel>("books", "witch", page: 2, pageSize: 2);

        first.TotalItems.Should().Be(3);
        first.Items.Select(b => b.Title).Should().Equal("Witcher two", "Witcher three");
        second.TotalItems.Should().Be(3);
        second.Items.Select(b => b.Title).Should().Equal("Witcher one");
    }

    [Fact]
    public async Task Authors_ShouldMatchNameOrPenName_InEnglishAndBulgarian()
    {
        var aleko = await this.Seed(d => d.SeedAuthor("Aleko Konstantinov", penName: "Щастливеца"));
        var king = await this.Seed(d => d.SeedAuthor("Stephen King"));

        var byPenName = await this.Search<SearchAuthorServiceModel>("authors", "ЩАСТ");
        var byName = await this.Search<SearchAuthorServiceModel>("authors", "steph");
        var byNameAndPenName = await this.Search<SearchAuthorServiceModel>("authors", "alek щаст");

        byPenName.Items.Select(a => a.Id).Should().Equal(aleko.Id);
        byName.Items.Select(a => a.Id).Should().Equal(king.Id);
        byNameAndPenName.Items.Select(a => a.Id).Should().Equal(aleko.Id);
    }

    [Fact]
    public async Task Profiles_ShouldMatchTermsAcrossFirstAndLastName()
    {
        var king = await this.Seed(d => d.SeedProfile("king-id", "Stephen", "King"));
        await this.Seed(d => d.SeedProfile("fry-id", "Stephen", "Fry"));
        var vazov = await this.Seed(d => d.SeedProfile("vazov-id", "Иван", "Вазов"));

        var both = await this.Search<SearchProfileServiceModel>("profiles", "ste kin");
        var bulgarian = await this.Search<SearchProfileServiceModel>("profiles", "иван ваз");
        var noMatch = await this.Search<SearchProfileServiceModel>("profiles", "ste vaz");

        both.Items.Select(p => p.Id).Should().Equal(king.UserId);
        bulgarian.Items.Select(p => p.Id).Should().Equal(vazov.UserId);
        noMatch.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Genres_ShouldMatchNamePrefix()
    {
        var fantasy = await this.Seed(d => d.SeedGenre("Fantasy"));
        var bulgarian = await this.Seed(d => d.SeedGenre("Фентъзи"));

        var english = await this.Search<SearchGenreServiceModel>("genres", "FAN");
        var cyrillic = await this.Search<SearchGenreServiceModel>("genres", "фен");

        english.Items.Select(g => g.Id).Should().Equal(fantasy.Id);
        cyrillic.Items.Select(g => g.Id).Should().Equal(bulgarian.Id);
    }

    [Fact]
    public async Task Articles_ShouldMatchTitleOrIntroduction_ForAnonymousCallers()
    {
        var article = await this.Seed(async d =>
        {
            var dbModel = new ArticleDbModel
            {
                Id = Guid.NewGuid(),
                Title = "Exploring Pet Sematary",
                Introduction = "Любов, загуба и цената на смъртта.",
                Content = new string('c', 200),
                ImagePath = "/images/articles/test.jpg",
            };

            d.Articles.Add(dbModel);
            await d.SaveChangesAsync();
            return dbModel;
        });

        var anonymous = this.factory.CreateAnonymousClient();

        var byTitle = await Search<SearchArticleServiceModel>(anonymous, "articles", "sema");
        var byIntroduction = await Search<SearchArticleServiceModel>(anonymous, "articles", "загу");

        byTitle.Items.Select(a => a.Id).Should().Equal(article.Id);
        byIntroduction.Items.Select(a => a.Id).Should().Equal(article.Id);
    }

    [Theory]
    [InlineData("books")]
    [InlineData("authors")]
    [InlineData("articles")]
    [InlineData("genres")]
    [InlineData("profiles")]
    public async Task OperatorOnlyInput_ShouldReturnAnEmptyPage_NotAnError(string endpoint)
    {
        await this.SeedOneOfEach();

        foreach (var term in new[] { "&|!():*", "' \\ <-> !!", ":*" })
        {
            var response = await this.client.GetAsync(Url(endpoint, term));

            response.StatusCode.Should().Be(HttpStatusCode.OK, $"term {term}");

            var page = await response.Content.ReadFromJsonAsync<Page<object>>();
            page!.Items.Should().BeEmpty($"term {term}");
            page.TotalItems.Should().Be(0, $"term {term}");
        }
    }

    [Theory]
    [InlineData("books")]
    [InlineData("authors")]
    [InlineData("articles")]
    [InlineData("genres")]
    [InlineData("profiles")]
    public async Task BlankInput_ShouldReturnEverything_AsBefore(string endpoint)
    {
        await this.SeedOneOfEach();

        var withoutTerm = await this.client.GetFromJsonAsync<Page<object>>(
            $"/Search/{endpoint}/");

        var blankTerm = await this.client.GetFromJsonAsync<Page<object>>(
            Url(endpoint, "   "));

        withoutTerm!.TotalItems.Should().BeGreaterThan(0);
        blankTerm!.TotalItems.Should().Be(withoutTerm.TotalItems);
    }

    [Fact]
    public async Task InputWithSqlAndTsQuerySyntax_ShouldBeTreatedAsPlainTerms()
    {
        var book = await this.Seed(d => d.SeedBook("O'Brien drops tables"));
        await this.Seed(d => d.SeedBook("Unrelated"));

        var page = await this.Search<SearchBookServiceModel>(
            "books",
            "o'brien'); DROP TABLE \"Books\"; --");

        page.Items.Should().BeEmpty();

        var apostrophe = await this.Search<SearchBookServiceModel>("books", "o'bri");
        apostrophe.Items.Select(b => b.Id).Should().Equal(book.Id);
    }

    private async Task SeedOneOfEach()
    {
        await this.Seed(d => d.SeedBook("Seed book"));
        await this.Seed(d => d.SeedAuthor("Seed author"));
        await this.Seed(d => d.SeedProfile("seed-profile", "Seed", "Profile"));
        await this.Seed(async d =>
        {
            d.Articles.Add(new ArticleDbModel
            {
                Id = Guid.NewGuid(),
                Title = "Seed article",
                Introduction = "Seed introduction",
                Content = new string('c', 200),
                ImagePath = "/images/articles/test.jpg",
            });

            return await d.SaveChangesAsync();
        });
    }

    private Task<T> Seed<T>(Func<BookHubDbContext, Task<T>> seed)
        => this.factory.WithData(seed);

    private Task<Page<T>> Search<T>(
        string endpoint,
        string term,
        int page = 1,
        int pageSize = 10)
        => Search<T>(this.client, endpoint, term, page, pageSize);

    private static async Task<Page<T>> Search<T>(
        HttpClient client,
        string endpoint,
        string term,
        int page = 1,
        int pageSize = 10)
    {
        var response = await client.GetAsync(Url(endpoint, term, page, pageSize));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return (await response.Content.ReadFromJsonAsync<Page<T>>())!;
    }

    private static string Url(
        string endpoint,
        string term,
        int page = 1,
        int pageSize = 10)
        => $"/Search/{endpoint}/?searchTerm={Uri.EscapeDataString(term)}&page={page}&pageSize={pageSize}";

    private sealed record Page<T>(
        List<T> Items,
        int TotalItems,
        int PageIndex,
        int PageSize);
}
