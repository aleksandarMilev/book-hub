namespace BookHub.Features.Authors.Web.User;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

using static Common.Constants.ApiRoutes;
using static Shared.Constants.RouteNames;

[Authorize]
public class AuthorController(IAuthorService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Top)]
    public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.TopThree(cancellationToken));

    [HttpGet(ApiRoutes.Names)]
    public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Names(cancellationToken));

    [HttpGet(Id, Name = DetailsRouteName)]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Details(
        Guid id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Details(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Create(
        CreateAuthorWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var result = await service.Create(
            serviceModel,
            cancellationToken);

        if (result.Succeeded)
        {
            return this.CreatedAtRoute(
                routeName: DetailsRouteName,
                routeValues: new { id = result.Data!.Id },
                value: result.Data);
        }

        return this.BadRequest(result.ErrorMessage);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateAuthorWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var result = await service.Edit(
            id,
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            id,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
