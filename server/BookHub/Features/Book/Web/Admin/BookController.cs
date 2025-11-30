namespace BookHub.Features.Book.Web.Admin;

using Areas.Admin.Web;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;

[Authorize]
public class BookController(IBookService service) : AdminApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<BookDetailsServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
        => this.Ok(await service.AdminDetails(id, token));

    [HttpPatch(Id + ApiRoutes.Approve)]
    public async Task<ActionResult> Approve(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Approve(id, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPatch(Id + ApiRoutes.Reject)]
    public async Task<ActionResult> Reject(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Reject(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
