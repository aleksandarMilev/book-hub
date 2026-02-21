namespace BookHub.Features.DataImporter.Web;

using Areas.Admin.Web;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

public class DataImporterController(IDataImporterService service) : AdminApiController
{
    [HttpPost(ApiRoutes.All)]
    public async Task<ActionResult<DataImportResult>> All(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportAll(cancellationToken));

    [HttpPost(ApiRoutes.Artciles)]
    public async Task<ActionResult<DataImportPartResult>> Articles(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportArticles(cancellationToken));

    [HttpPost(ApiRoutes.Authors)]
    public async Task<ActionResult<DataImportPartResult>> Authors(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportAuthors(cancellationToken));

    [HttpPost(ApiRoutes.Books)]
    public async Task<ActionResult<DataImportPartResult>> Books(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportBooks(cancellationToken));

    [HttpPost(ApiRoutes.Genres)]
    public async Task<ActionResult<DataImportPartResult>> Genres(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportGenres(cancellationToken));

    [HttpPost(ApiRoutes.BooksGenres)]
    public async Task<ActionResult<DataImportPartResult>> BooksGenres(
        CancellationToken cancellationToken)
        => this.Ok(await service.ImportBooksGenres(cancellationToken));
}
