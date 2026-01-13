namespace BookHub.Features.Authors.Web.Admin;

using Areas.Admin.Web;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static Common.Constants.ApiRoutes;

[Authorize]
public class AuthorController(IAuthorService service) : AdminApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.AdminDetails(id, cancellationToken));

    [HttpPatch(Id + ApiRoutes.Approve)]
    public async Task<ActionResult> Approve(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Approve(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPatch((Id + ApiRoutes.Reject))]
    public async Task<ActionResult> Reject(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Reject(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
