namespace BookHub.Tests.Catalog;

using System.Net;
using System.Net.Http.Json;
using Common;
using Data;
using Features.Authors.Data.Models;
using Features.Authors.Service.Models;
using Features.Books.Data.Models;
using Features.Books.Service.Models;
using Features.Genres.Service.Models;
using Features.ReadingLists.Data.Models;
using Features.ReadingLists.Shared;
using Features.Reviews.Service.Models;
using Features.Search.Service.Models;
using FluentAssertions;
using Shared.Seed;
using Shared.Utils;

// The catalog is publicly readable (Phase 2c). An anonymous visitor sees exactly what a user who
// isn't the creator sees: approved, non-deleted rows, with no user-specific data and no pending edits.
public sealed class PublicCatalogIntegration : IAsyncLifetime
{
    private const string CreatorId = "catalog-creator";
    private const string ReaderId = "catalog-reader";
    private const string AdminId = "test-admin-id";

    private readonly BookHubWebApplicationFactory factory = new();

    private HttpClient anonymous = null!;

    public async ValueTask InitializeAsync()
    {
        await this.factory.ResetDatabase();
        await this.factory.WithData(async data =>
        {
            await data.SeedUser(CreatorId, "creator");
            await data.SeedUser(ReaderId, "reader");
        });

        this.anonymous = this.factory.CreateAnonymousClient();
    }

    public async ValueTask DisposeAsync()
        => await this.factory.DisposeAsync();

    [Fact]
    public async Task BookDetails_ShouldReturnTheBookWithReviews_AndNoReadingStatus_WhenAnonymous()
    {
        var (book, _) = await this.SeedReviewedBook();

        var response = await this.anonymous.GetAsync($"/Books/{book.Id}/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.Content.ReadFromJsonAsync<BookDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);
        details!.Title.Should().Be("Public book");
        details.Author!.Name.Should().Be("Public author");
        details.Reviews.Select(r => r.CreatorId).Should().BeEquivalentTo([CreatorId, ReaderId]);
        details.ReadingStatus.Should().BeNull();
    }

    [Fact]
    public async Task BookDetails_ShouldStillReturnTheCallersReadingStatus_WhenLoggedIn()
    {
        var (book, _) = await this.SeedReviewedBook();
        var reader = this.factory.CreateUserClient(ReaderId, "reader");

        var details = await reader.GetFromJsonAsync<BookDetailsServiceModel>($"/Books/{book.Id}/", cancellationToken: TestContext.Current.CancellationToken);

        details!.Reviews.Should().HaveCount(2);
        details.ReadingStatus.Should().Be((int)ReadingListStatus.CurrentlyReading);
    }

    [Fact]
    public async Task BooksByGenreAndByAuthor_ShouldReturnOnlyApprovedBooks_WhenAnonymous()
    {
        var (genreId, authorId, approvedId) = await this.factory.WithData(async data =>
        {
            var genre = await data.SeedGenre("Public genre");
            var author = await data.SeedAuthor("Public author");
            var approved = await data.SeedBook("Approved book", authorId: author.Id, genreIds: [genre.Id]);
            await data.SeedBook("Unapproved book", isApproved: false, creatorId: CreatorId, authorId: author.Id, genreIds: [genre.Id]);
            await SeedDeletedBook(data, "Deleted book", author.Id, genre.Id);

            return (genre.Id, author.Id, approved.Id);
        });

        var byGenre = await this.anonymous.GetFromJsonAsync<PaginatedModel<BookServiceModel>>($"/Books/genre/{genreId}/", cancellationToken: TestContext.Current.CancellationToken);
        var byAuthor = await this.anonymous.GetFromJsonAsync<PaginatedModel<BookServiceModel>>($"/Books/author/{authorId}/", cancellationToken: TestContext.Current.CancellationToken);

        byGenre!.Items.Select(b => b.Id).Should().Equal(approvedId);
        byGenre.TotalItems.Should().Be(1);
        byAuthor!.Items.Select(b => b.Id).Should().Equal(approvedId);
        byAuthor.TotalItems.Should().Be(1);
    }

    [Fact]
    public async Task AuthorDetails_ShouldReturnTheAuthorWithOnlyApprovedTopBooks_WhenAnonymous()
    {
        var (authorId, approvedId) = await this.factory.WithData(async data =>
        {
            var author = await data.SeedAuthor("Public author");
            var approved = await data.SeedBook("Approved book", authorId: author.Id);
            await data.SeedBook("Unapproved book", isApproved: false, creatorId: CreatorId, authorId: author.Id);
            await SeedDeletedBook(data, "Deleted book", author.Id);

            return (author.Id, approved.Id);
        });

        var response = await this.anonymous.GetAsync($"/Authors/{authorId}/", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var details = await response.Content.ReadFromJsonAsync<AuthorDetailsServiceModel>(cancellationToken: TestContext.Current.CancellationToken);
        details!.Name.Should().Be("Public author");
        details.TopBooks.Select(b => b.Id).Should().Equal(approvedId);
    }

    [Fact]
    public async Task Genres_ShouldListGenres_AndGenreDetailsShouldShowOnlyApprovedTopBooks_WhenAnonymous()
    {
        var (genreId, approvedId) = await this.factory.WithData(async data =>
        {
            var genre = await data.SeedGenre("Public genre");
            var approved = await data.SeedBook("Approved book", genreIds: [genre.Id]);
            await data.SeedBook("Unapproved book", isApproved: false, creatorId: CreatorId, genreIds: [genre.Id]);
            await SeedDeletedBook(data, "Deleted book", genreId: genre.Id);

            return (genre.Id, approved.Id);
        });

        var names = await this.anonymous.GetFromJsonAsync<GenreNameServiceModel[]>("/Genres/", cancellationToken: TestContext.Current.CancellationToken);
        var details = await this.anonymous.GetFromJsonAsync<GenreDetailsServiceModel>($"/Genres/{genreId}/", cancellationToken: TestContext.Current.CancellationToken);

        names!.Select(g => g.Id).Should().Contain([genreId, TestSeeder.OtherGenreId]);
        details!.Name.Should().Be("Public genre");
        details.TopBooks.Select(b => b.Id).Should().Equal(approvedId);
    }

    [Fact]
    public async Task Reviews_ShouldListTheBooksReviews_AndReturnASingleReview_WhenAnonymous()
    {
        var (book, review) = await this.SeedReviewedBook();

        var page = await this.anonymous.GetFromJsonAsync<PaginatedModel<ReviewServiceModel>>($"/Reviews/book/{book.Id}/", cancellationToken: TestContext.Current.CancellationToken);
        var single = await this.anonymous.GetFromJsonAsync<ReviewServiceModel>($"/Reviews/{review.Id}/", cancellationToken: TestContext.Current.CancellationToken);

        page!.Items.Select(r => r.CreatorId).Should().BeEquivalentTo([CreatorId, ReaderId]);
        page.TotalItems.Should().Be(2);
        single!.Id.Should().Be(review.Id);
        single.BookId.Should().Be(book.Id);
    }

    [Fact]
    public async Task Searches_ShouldReturnOnlyApprovedNonDeletedRows_WhenAnonymous()
    {
        var (bookId, authorId, genreId) = await this.factory.WithData(async data =>
        {
            var genre = await data.SeedGenre("Zephyrgenre");
            var deletedGenre = await data.SeedGenre("Zephyrgone");
            data.Remove(deletedGenre);

            var author = await data.SeedAuthor("Zephyr Approved");
            await data.SeedAuthor("Zephyr Unapproved", isApproved: false, creatorId: CreatorId);

            var book = await data.SeedBook("Zephyr approved");
            await data.SeedBook("Zephyr unapproved", isApproved: false, creatorId: CreatorId);
            await SeedDeletedBook(data, "Zephyr deleted");

            return (book.Id, author.Id, genre.Id);
        });

        var books = await this.anonymous.GetFromJsonAsync<PaginatedModel<SearchBookServiceModel>>("/Search/books/?searchTerm=zephyr", cancellationToken: TestContext.Current.CancellationToken);
        var authors = await this.anonymous.GetFromJsonAsync<PaginatedModel<SearchAuthorServiceModel>>("/Search/authors/?searchTerm=zephyr", cancellationToken: TestContext.Current.CancellationToken);
        var genres = await this.anonymous.GetFromJsonAsync<PaginatedModel<SearchGenreServiceModel>>("/Search/genres/?searchTerm=zephyr", cancellationToken: TestContext.Current.CancellationToken);

        books!.Items.Select(b => b.Id).Should().Equal(bookId);
        authors!.Items.Select(a => a.Id).Should().Equal(authorId);
        genres!.Items.Select(g => g.Id).Should().Equal(genreId);
    }

    [Fact]
    public async Task UnapprovedBookAndAuthor_ShouldBeNotFound_WhenAnonymous()
    {
        var (bookId, authorId) = await this.factory.WithData(async data =>
        {
            var author = await data.SeedAuthor("Unapproved author", isApproved: false, creatorId: CreatorId);
            var book = await data.SeedBook("Unapproved book", isApproved: false, creatorId: CreatorId);

            return (book.Id, author.Id);
        });

        await (await this.anonymous.GetAsync($"/Books/{bookId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
        await (await this.anonymous.GetAsync($"/Authors/{authorId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SoftDeletedRows_ShouldBeNotFoundOrAbsent_WhenAnonymous()
    {
        var (book, review) = await this.SeedReviewedBook();
        var (deletedBookId, deletedAuthorId, deletedGenreId) = await this.factory.WithData(async data =>
        {
            var deletedBook = await SeedDeletedBook(data, "Deleted book");
            var deletedAuthor = await data.SeedAuthor("Deleted author");
            var deletedGenre = await data.SeedGenre("Deleted genre");
            var deletedReview = await data.FindAsync<Features.Reviews.Data.Models.ReviewDbModel>(review.Id);

            data.RemoveRange(deletedAuthor, deletedGenre, deletedReview!);
            await data.SaveChangesAsync();

            return (deletedBook.Id, deletedAuthor.Id, deletedGenre.Id);
        });

        await (await this.anonymous.GetAsync($"/Books/{deletedBookId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
        await (await this.anonymous.GetAsync($"/Authors/{deletedAuthorId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
        await (await this.anonymous.GetAsync($"/Genres/{deletedGenreId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
        await (await this.anonymous.GetAsync($"/Reviews/{review.Id}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);

        var genres = await this.anonymous.GetFromJsonAsync<GenreNameServiceModel[]>("/Genres/", cancellationToken: TestContext.Current.CancellationToken);
        var reviews = await this.anonymous.GetFromJsonAsync<PaginatedModel<ReviewServiceModel>>($"/Reviews/book/{book.Id}/", cancellationToken: TestContext.Current.CancellationToken);

        genres!.Select(g => g.Id).Should().NotContain(deletedGenreId);
        reviews!.Items.Select(r => r.Id).Should().NotContain(review.Id).And.HaveCount(1);
    }

    [Fact]
    public async Task ReviewsOfAnUnapprovedBook_ShouldBeNotFound_ForAnonymousAndNonCreators()
    {
        var (bookId, reviewId) = await this.SeedReviewedUnapprovedBook();
        var nonCreator = this.factory.CreateUserClient(ReaderId, "reader");

        foreach (var client in new[] { this.anonymous, nonCreator })
        {
            await (await client.GetAsync($"/Reviews/book/{bookId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
            await (await client.GetAsync($"/Reviews/{reviewId}/", TestContext.Current.CancellationToken)).ShouldBeProblem(HttpStatusCode.NotFound);
        }
    }

    [Fact]
    public async Task ReviewsOfAnUnapprovedBook_ShouldBeVisible_ToTheCreatorAndAdmins()
    {
        var (bookId, reviewId) = await this.SeedReviewedUnapprovedBook();
        var creator = this.factory.CreateUserClient(CreatorId, "creator");
        var admin = this.factory.CreateAdminClient(AdminId);

        foreach (var client in new[] { creator, admin })
        {
            var page = await client.GetFromJsonAsync<PaginatedModel<ReviewServiceModel>>($"/Reviews/book/{bookId}/", cancellationToken: TestContext.Current.CancellationToken);
            var single = await client.GetFromJsonAsync<ReviewServiceModel>($"/Reviews/{reviewId}/", cancellationToken: TestContext.Current.CancellationToken);

            page!.Items.Select(r => r.Id).Should().Equal(reviewId);
            single!.Id.Should().Be(reviewId);
        }
    }

    [Fact]
    public async Task ReviewsOfAMissingBook_ShouldBeNotFound()
    {
        var response = await this.anonymous.GetAsync($"/Reviews/book/{Guid.NewGuid()}/", TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(CreatorId)]
    [InlineData(null)]
    public async Task BookDetails_ShouldNotOverlayAPendingEdit_WhenAnonymous(string? creatorId)
    {
        // A null CreatorId (imported data) must not make an anonymous caller (null ID) the creator.
        var bookId = await this.factory.WithData(async data =>
        {
            var book = await data.SeedBook("Original title", creatorId: creatorId);
            data.BookEdits.Add(new BookEditDbModel
            {
                BookId = book.Id,
                RequestedById = CreatorId,
                Title = "Pending title",
                ShortDescription = book.ShortDescription,
                LongDescription = book.LongDescription,
                ImagePath = book.ImagePath,
            });

            await data.SaveChangesAsync();

            return book.Id;
        });

        var details = await this.anonymous.GetFromJsonAsync<BookDetailsServiceModel>($"/Books/{bookId}/", cancellationToken: TestContext.Current.CancellationToken);

        details!.Title.Should().Be("Original title");

        if (creatorId is not null)
        {
            var creator = this.factory.CreateUserClient(CreatorId, "creator");
            var creatorView = await creator.GetFromJsonAsync<BookDetailsServiceModel>($"/Books/{bookId}/", cancellationToken: TestContext.Current.CancellationToken);

            creatorView!.Title.Should().Be("Pending title");
        }
    }

    [Theory]
    [InlineData(CreatorId)]
    [InlineData(null)]
    public async Task AuthorDetails_ShouldNotOverlayAPendingEdit_WhenAnonymous(string? creatorId)
    {
        var authorId = await this.factory.WithData(async data =>
        {
            var author = await data.SeedAuthor("Original name", creatorId: creatorId);
            data.AuthorEdits.Add(new AuthorEditDbModel
            {
                AuthorId = author.Id,
                RequestedById = CreatorId,
                Name = "Pending name",
                Biography = author.Biography,
                ImagePath = author.ImagePath,
                Nationality = author.Nationality,
                Gender = author.Gender,
            });

            await data.SaveChangesAsync();

            return author.Id;
        });

        var details = await this.anonymous.GetFromJsonAsync<AuthorDetailsServiceModel>($"/Authors/{authorId}/", cancellationToken: TestContext.Current.CancellationToken);

        details!.Name.Should().Be("Original name");

        if (creatorId is not null)
        {
            var creator = this.factory.CreateUserClient(CreatorId, "creator");
            var creatorView = await creator.GetFromJsonAsync<AuthorDetailsServiceModel>($"/Authors/{authorId}/", cancellationToken: TestContext.Current.CancellationToken);

            creatorView!.Name.Should().Be("Pending name");
        }
    }

    [Theory]
    [InlineData("/Profile/mine/")]
    [InlineData("/Profile/" + CreatorId + "/")]
    [InlineData("/Search/profiles/?searchTerm=creator")]
    [InlineData("/Authors/names/")]
    public async Task ProfilesAndAuthorNames_ShouldStillRequireLogin(string url)
    {
        var response = await this.anonymous.GetAsync(url, TestContext.Current.CancellationToken);

        await response.ShouldBeProblem(HttpStatusCode.Unauthorized);
    }

    private static async Task<BookDbModel> SeedDeletedBook(
        BookHubDbContext data,
        string title,
        Guid? authorId = null,
        Guid? genreId = null)
    {
        var book = genreId is null
            ? await data.SeedBook(title, authorId: authorId)
            : await data.SeedBook(title, authorId: authorId, genreIds: [genreId.Value]);

        data.Remove(book);
        await data.SaveChangesAsync();

        return book;
    }

    // An approved book by an approved author, reviewed by the creator and the reader, and on
    // the reader's "currently reading" list. Returns the book and the creator's review.
    private Task<(BookDbModel Book, Features.Reviews.Data.Models.ReviewDbModel Review)> SeedReviewedBook()
        => this.factory.WithData(async data =>
        {
            var author = await data.SeedAuthor("Public author");
            var book = await data.SeedBook("Public book", creatorId: CreatorId, authorId: author.Id);
            var review = await data.SeedReview(book.Id, CreatorId);
            await data.SeedReview(book.Id, ReaderId);

            data.ReadingLists.Add(new ReadingListDbModel
            {
                UserId = ReaderId,
                BookId = book.Id,
                Status = ReadingListStatus.CurrentlyReading,
            });

            await data.SaveChangesAsync();

            return (book, review);
        });

    private Task<(Guid BookId, Guid ReviewId)> SeedReviewedUnapprovedBook()
        => this.factory.WithData(async data =>
        {
            var book = await data.SeedBook("Pending book", isApproved: false, creatorId: CreatorId);
            var review = await data.SeedReview(book.Id, CreatorId);

            return (book.Id, review.Id);
        });
}
