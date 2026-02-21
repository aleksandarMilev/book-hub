namespace BookHub.Features.DataImporter.Service;

using System.Text.Json;
using Articles.Data.Models;
using Authors.Data.Models;
using BookHub.Data;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Books.Data.Models;
using Genres.Data.Models;
using Microsoft.EntityFrameworkCore;
using Models;

public sealed class DataImporterService(
    BookHubDbContext data,
    IHostEnvironment env) : IDataImporterService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<DataImportPartResult> ImportArticles(
        CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(
            GetDataRoot(),
            "articles.json");

        var articles = LoadJson<List<ArticleDbModel>>(path) ?? [];
        var articleIncomingIds = articles
          .Select(a => a.Id)
          .ToList();

        var articleExistingIds = articleIncomingIds.Count == 0
            ? []
            : (await data.Articles
                    .IgnoreQueryFilters()
                    .Where(a => articleIncomingIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken))
                .ToHashSet();

        var articlesToInsert = articles
            .Where(a => !articleExistingIds.Contains(a.Id))
            .ToList();

        await BatchInsertAsync(
            data.Articles,
            articlesToInsert,
            cancellationToken);

        return new(
            Entity: "Articles",
            TotalInFile: articles.Count,
            Inserted: articlesToInsert.Count,
            SkippedExisting: articles.Count - articlesToInsert.Count,
            SkippedInvalid: 0);
    }

    public async Task<DataImportPartResult> ImportAuthors(
        CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(
            GetDataRoot(),
            "authors.json");

        var authors = LoadJson<List<AuthorDbModel>>(path) ?? [];
        var authorIncomingIds = authors
              .Select(a => a.Id)
              .ToList();

        var authorExistingIds = authorIncomingIds.Count == 0
            ? []
            : (await data
                    .Authors
                    .IgnoreQueryFilters()
                    .Where(a => authorIncomingIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken))
                .ToHashSet();

        var authorsToInsert = authors
            .Where(a => !authorExistingIds.Contains(a.Id))
            .ToList();

        await BatchInsertAsync(
            data.Authors,
            authorsToInsert,
            cancellationToken);

        return new(
            Entity: "Authors",
            TotalInFile: authors.Count,
            Inserted: authorsToInsert.Count,
            SkippedExisting: authors.Count - authorsToInsert.Count,
            SkippedInvalid: 0);
    }

    public async Task<DataImportPartResult> ImportBooks(
        CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(
            GetDataRoot(),
            "books.json");

        var books = LoadJson<List<BookDbModel>>(path) ?? [];

        var allAuthorIds = (await data
                .Authors
                .IgnoreQueryFilters()
                .Select(a => a.Id)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var validBooks = books
            .Where(b =>
                b.AuthorId is null ||
                allAuthorIds.Contains(b.AuthorId.Value))
            .ToList();

        var skippedInvalidBooks = books.Count - validBooks.Count;

        var bookIncomingIds = validBooks
            .Select(b => b.Id)
            .ToList();

        var bookExistingIds = bookIncomingIds.Count == 0
            ? []
            : (await data.Books
                    .IgnoreQueryFilters()
                    .Where(b => bookIncomingIds.Contains(b.Id))
                    .Select(b => b.Id)
                    .ToListAsync(cancellationToken))
                .ToHashSet();

        var booksToInsert = validBooks
            .Where(b => !bookExistingIds.Contains(b.Id))
            .ToList();

        await BatchInsertAsync(
            data.Books,
            booksToInsert,
            cancellationToken);

        return new(
            Entity: "Books",
            TotalInFile: books.Count,
            Inserted: booksToInsert.Count,
            SkippedExisting: validBooks.Count - booksToInsert.Count,
            SkippedInvalid: skippedInvalidBooks);
    }

    public async Task<DataImportPartResult> ImportGenres(
        CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(
            GetDataRoot(),
            "genres.json");

        var genres = LoadJson<List<GenreDbModel>>(path) ?? [];
        var genreIncomingIds = genres
            .Select(g => g.Id)
            .ToList();

        var genreExistingIds = genreIncomingIds.Count == 0
            ? []
            : (await data.Genres
                    .IgnoreQueryFilters()
                    .Where(g => genreIncomingIds.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToListAsync(cancellationToken))
                .ToHashSet();

        var genresToInsert = genres
            .Where(g => !genreExistingIds.Contains(g.Id))
            .ToList();

        await BatchInsertAsync(
            data.Genres,
            genresToInsert,
            cancellationToken);

        return new(
            Entity: "Genres",
            TotalInFile: genres.Count,
            Inserted: genresToInsert.Count,
            SkippedExisting: genres.Count - genresToInsert.Count,
            SkippedInvalid: 0);
    }

    public async Task<DataImportPartResult> ImportBooksGenres(
        CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(
            GetDataRoot(),
            "books_genres.json");

        var booksGenres = LoadJson<List<BookGenreDbModel>>(path) ?? [];
        var allBookIds = (await data.Books
                .IgnoreQueryFilters()
                .Select(b => b.Id)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var allGenreIds = (await data.Genres
                .IgnoreQueryFilters()
                .Select(g => g.Id)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var validBookGenres = booksGenres
            .Where(bg =>
                allBookIds.Contains(bg.BookId) &&
                allGenreIds.Contains(bg.GenreId))
            .ToList();

        var skippedInvalidBookGenres = booksGenres.Count - validBookGenres.Count;

        var involvedBookIds = validBookGenres
            .Select(bg => bg.BookId)
            .Distinct()
            .ToList();

        var existingPairs = involvedBookIds.Count == 0
            ? []
            : await data.BooksGenres
                .IgnoreQueryFilters()
                .Where(bg => involvedBookIds.Contains(bg.BookId))
                .Select(bg => new { bg.BookId, bg.GenreId })
                .ToListAsync(cancellationToken);

        var existingPairSet = existingPairs
            .Select(x => (x.BookId, x.GenreId))
            .ToHashSet();

        var bookGenresToInsert = validBookGenres
            .Where(bg => !existingPairSet.Contains((bg.BookId, bg.GenreId)))
            .ToList();

        await this.BatchInsertAsync(
            data.BooksGenres,
            bookGenresToInsert,
            cancellationToken);

        return new(
            Entity: "BookGenres",
            TotalInFile: booksGenres.Count,
            Inserted: bookGenresToInsert.Count,
            SkippedExisting: validBookGenres.Count - bookGenresToInsert.Count,
            SkippedInvalid: skippedInvalidBookGenres);

    }

    public async Task<DataImportResult> ImportAll(
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await data
            .Database
            .BeginTransactionAsync(cancellationToken);

        var authorsPart = await this.ImportAuthors(cancellationToken);
        var genresPart = await this.ImportGenres(cancellationToken);
        var booksPart = await this.ImportBooks(cancellationToken);
        var booksGenresPart = await this.ImportBooksGenres(cancellationToken);
        var articlesPart = await this.ImportArticles(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new(
            Authors: authorsPart,
            Genres: genresPart,
            Books: booksPart,
            BooksGenres: booksGenresPart,
            Articles: articlesPart);
    }

    private string GetDataRoot()
        => Path.Combine(
            env.ContentRootPath,
            "Features",
            "DataImporter",
            "Data");

    private static T? LoadJson<T>(string path)
    {
        if (!File.Exists(path))
        {
            return default;
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(
            json,
            JsonOptions);
    }

    private async Task BatchInsertAsync<T>(
        DbSet<T> dbSet,
        List<T> items,
        CancellationToken cancellationToken) where T : class
    {
        if (items.Count == 0)
        {
            return;
        }

        data
            .ChangeTracker
            .AutoDetectChangesEnabled = false;

        const int batchSize = 1_000;

        for (var i = 0; i < items.Count; i += batchSize)
        {
            var batch = items
                .Skip(i)
                .Take(batchSize);

            dbSet.AddRange(batch);

            await data.SaveChangesAsync(cancellationToken);

            data
                .ChangeTracker
                .Clear();
        }

        data
            .ChangeTracker
            .AutoDetectChangesEnabled = true;
    }
}