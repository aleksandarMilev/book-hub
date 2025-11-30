namespace BookHub.Features.Authors.Web.Admin;

using Areas.Admin.Web;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;

using static ApiRoutes;
using static Common.Constants.ApiRoutes;

[Authorize]
public class AuthorController(IAuthorService service) : AdminApiController
{
    [HttpGet(Id)]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
      => this.Ok(await service.AdminDetails(id, token));

    [HttpPatch(Id + Author.Approve)]
    public async Task<ActionResult> Approve(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Approve(id, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpPatch((Id + Author.Reject))]
    public async Task<ActionResult> Reject(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Reject(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
