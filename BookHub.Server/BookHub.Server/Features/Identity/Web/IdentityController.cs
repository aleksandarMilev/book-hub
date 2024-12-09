namespace BookHub.Server.Features.Identity.Web
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;

    public class IdentityController(IIdentityService service) : ApiController
    {
        private readonly IIdentityService service = service;

        [HttpPost(ApiRoutes.Register)]
        public async Task<ActionResult<LoginResponseModel>> Register(RegisterRequestModel model)
        {
            var result = await this.service.RegisterAsync(
                model.Email,
                model.Username,
                model.Password);

            if (result.Succeeded)
            {
                return this.Ok(new LoginResponseModel(result.Data!));
            }

            return this.Unauthorized(new { errorMessage = result.ErrorMessage });
        }

        [HttpPost(ApiRoutes.Login)]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var result = await this.service.LoginAsync(
                 model.Credentials,
                 model.Password,
                 model.RememberMe);

            if (result.Succeeded)
            {
                return this.Ok(new LoginResponseModel(result.Data!));
            }

            return this.Unauthorized(new { errorMessage = result.ErrorMessage });
        }
    }
}
