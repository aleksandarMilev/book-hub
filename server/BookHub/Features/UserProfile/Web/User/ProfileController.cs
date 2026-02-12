namespace BookHub.Features.UserProfile.Web.User;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

[Authorize]
public class ProfileController(IProfileService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Top)]
    public async Task<ActionResult<IEnumerable<ProfileServiceModel>>> TopThree(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.TopThree(cancellationToken));

    [HttpGet(ApiRoutes.Mine, Name = nameof(Mine))]
    public async Task<ActionResult<ProfileServiceModel>> Mine(
        CancellationToken cancellationToken = default)
        => this.Ok(await service.Mine(cancellationToken));

    [HttpGet(Common.Constants.ApiRoutes.Id)]
    public async Task<ActionResult<IProfileServiceModel>> OtherUser(
        string id,
        CancellationToken cancellationToken = default)
        => this.Ok(await service.OtherUser(id, cancellationToken));

    [HttpPut]
    public async Task<ActionResult> Edit(
        CreateProfileWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var result = await service.Edit(
            serviceModel,
            cancellationToken);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(
        CancellationToken cancellationToken = default)
    {
        var result = await service.Delete(
            cancellationToken: cancellationToken);

        return this.NoContentOrBadRequest(result);
    }
}
