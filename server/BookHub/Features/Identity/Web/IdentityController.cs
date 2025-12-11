namespace BookHub.Features.Identity.Web;

using BookHub.Common;
using Identity.Shared;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using Service.Models;

public class IdentityController(IIdentityService service) : ApiController
{
    [HttpPost(ApiRoutes.Register)]
    public async Task<ActionResult<LoginResponseModel>> Register(
        RegisterWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToRegisterServiceModel();
        var result = await service.Register(serviceModel, token);

        return this.OkOrBadRequest(
            result,
            token => new LoginResponseModel(token));
    }

    [HttpPost(ApiRoutes.Login)]
    public async Task<ActionResult<LoginResponseModel>> Login(
        LoginWebModel webModel,
        CancellationToken token = default)
    {
        var serviceModel = webModel.ToLoginServiceModel();
        var result = await service.Login(serviceModel, token);

        return this.OkOrBadRequest(
            result,
            token => new LoginResponseModel(token));
    }
}
