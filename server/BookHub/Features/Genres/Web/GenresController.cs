namespace BookHub.Features.Genres.Web;

using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;

[Authorize]
public class GenresController(IGenreService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Names(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Names(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Id)]
    public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, cancellationToken));
}
