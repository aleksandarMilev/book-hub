namespace BookHub.Features.Authors.Web.User;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

using static ApiRoutes;
using static Common.Constants.ApiRoutes;

[Authorize]
public class AuthorController(IAuthorService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(Author.Top)]
    public async Task<ActionResult<IEnumerable<AuthorServiceModel>>> TopThree(
        CancellationToken token = default)
        => this.Ok(await service.TopThree(token));

    [HttpGet(Author.Names)]
    public async Task<ActionResult<IEnumerable<AuthorNamesServiceModel>>> Names(
        CancellationToken token = default)
        => this.Ok(await service.Names(token));

    [HttpGet(Id, Name = nameof(this.Details))]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Details(
        Guid id,
        CancellationToken token = default)
        => this.Ok(await service.Details(id, token));

    [HttpPost]
    public async Task<ActionResult<AuthorDetailsServiceModel>> Create(
        CreateAuthorWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var createdAuthor = await service.Create(serviceModel, token);

        return this.CreatedAtRoute(
            routeName: nameof(this.Details),
            routeValues: new { id = createdAuthor.Id },
            value: createdAuthor);
    }

    [HttpPut(Id)]
    public async Task<ActionResult> Edit(
        Guid id,
        CreateAuthorWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var result = await service.Edit(id, serviceModel, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete(Id)]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var result = await service.Delete(id, token);

        return this.NoContentOrBadRequest(result);
    }
}
