namespace BookHub.Features.Genre.Web;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;

[Authorize]
public class GenresController(IGenreService service) : ApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Names(
        CancellationToken token = default)
       => this.Ok(await service.Names(token));

    [HttpGet(Id)]
    public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Details(
        Guid id,
        CancellationToken token = default)
       => this.Ok(await service.Details(id, token));
}
