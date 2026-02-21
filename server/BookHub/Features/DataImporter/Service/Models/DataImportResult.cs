namespace BookHub.Features.DataImporter.Service.Models;

public sealed record DataImportResult(
    DataImportPartResult Authors,
    DataImportPartResult Genres,
    DataImportPartResult Books,
    DataImportPartResult BooksGenres,
    DataImportPartResult Articles);