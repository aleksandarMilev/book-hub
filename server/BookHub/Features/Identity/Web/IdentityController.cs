namespace BookHub.Features.Identity.Web;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;

public class IdentityController(IIdentityService service) : ApiController
{
    private readonly IIdentityService service = service;

    [HttpPost(ApiRoutes.Register)]
    public async Task<ActionResult<LoginResponseModel>> Register(RegisterRequestModel model)
    {
        var result = await this.service.Register(
            model.Email,
            model.Username,
            model.Password);

        return this.OkOrBadRequest(
            result,
            token => new LoginResponseModel(token));
    }

    [HttpPost(ApiRoutes.Login)]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
    {
        var result = await this.service.Login(
             model.Credentials,
             model.Password,
             model.RememberMe);

        return this.OkOrBadRequest(
            result,
            token => new LoginResponseModel(token));
    }
}
