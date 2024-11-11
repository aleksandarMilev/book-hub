namespace BookHub.Server.Features.Identity.Web
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;
    using Service;

    using static BookHub.Server.Common.Messages.Error.Identity;

    public class IdentityController(
        IIdentityService identityService,
        UserManager<User> userManager,
        IOptions<AppSettings> appSettings) : ApiController
    {
        private readonly IIdentityService identityService = identityService;
        private readonly UserManager<User> userManager = userManager;
        private readonly AppSettings appSettings = appSettings.Value;

        [HttpPost(nameof(this.Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = this.identityService.GenerateJwtToken(this.appSettings.Secret, user.Id, user.UserName!);

                return this.Ok(new LoginResponseModel(user.UserName!, user.Email!, user.Id, token));
            }

            var errorMessage = result
                .Errors
                .Select(e => e.Description)
                .ToList();

            return this.Unauthorized(new { errorMessage });
        }


        [HttpPost(nameof(this.Login))]
        public async Task<ActionResult> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return this.Unauthorized(new { errorMessage = InvalidLoginAttempt });
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (passwordIsValid)
            {
                var token = this.identityService.GenerateJwtToken(this.appSettings.Secret, user.Id, user.UserName!);

                return this.Ok(new LoginResponseModel(user.UserName!, user.Email!, user.Id, token));
            }

            return this.Unauthorized(new { errorMessage = InvalidLoginAttempt });
        }
    }
}
