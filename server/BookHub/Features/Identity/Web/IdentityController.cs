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
    private const string RegisterRoute = "register/";

    private const string LoginRoute = "login/";

    [HttpPost(RegisterRoute)]
    public async Task<ActionResult<JwtTokenServiceModel>> Register(
        RegisterWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToRegisterServiceModel();
        var result = await service.Register(
            serviceModel,
            cancellationToken);

        return this.OkOrBadRequest(
            result,
            token => new JwtTokenServiceModel(token));
    }

    [HttpPost(LoginRoute)]
    public async Task<ActionResult<JwtTokenServiceModel>> Login(
        LoginWebModel webModel,
        CancellationToken cancellationToken = default)
    {
        var serviceModel = webModel.ToLoginServiceModel();
        var result = await service.Login(
            serviceModel,
            cancellationToken);

        return this.OkOrBadRequest(
            result,
            token => new JwtTokenServiceModel(token));
    }
}
