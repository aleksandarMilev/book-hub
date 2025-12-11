namespace BookHub.Features.UserProfile.Web.User;

using BookHub.Common;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;
using Shared;

using static Common.Constants.ApiRoutes;

[Authorize]
public class ProfileController(IProfileService service) : ApiController
{
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Top)]
    public async Task<ActionResult<IEnumerable<ProfileServiceModel>>> TopThree(
        CancellationToken token = default)
        => this.Ok(await service.TopThree(token));

    [HttpGet(ApiRoutes.Mine, Name = nameof(this.Mine))]
    public async Task<ActionResult<ProfileServiceModel>> Mine(
        CancellationToken token = default)
        => this.Ok(await service.Mine(token));

    [HttpGet(Id)]
    public async Task<ActionResult<IProfileServiceModel>> OtherUser(
        string id,
        CancellationToken token = default)
        => this.Ok(await service.OtherUser(id, token));

    [HttpPut]
    public async Task<ActionResult> Edit(
        CreateProfileWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToCreateServiceModel();
        var result = await service.Edit(serviceModel, token);

        return this.NoContentOrBadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(
        CancellationToken token = default)
    {
        var result = await service.Delete(token: token);

        return this.NoContentOrBadRequest(result);
    }
}
