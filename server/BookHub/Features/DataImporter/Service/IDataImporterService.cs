namespace BookHub.Features.DataImporter.Service;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IDataImporterService : ITransientService
{
    Task<DataImportResult> ImportAll(
        CancellationToken cancellationToken = default);

    Task<DataImportPartResult> ImportArticles(
        CancellationToken cancellationToken = default);

    Task<DataImportPartResult> ImportAuthors(
        CancellationToken cancellationToken = default);

    Task<DataImportPartResult> ImportBooks(
        CancellationToken cancellationToken = default);

    Task<DataImportPartResult> ImportGenres(
        CancellationToken cancellationToken = default);

    Task<DataImportPartResult> ImportBooksGenres(
        CancellationToken cancellationToken = default);
}
