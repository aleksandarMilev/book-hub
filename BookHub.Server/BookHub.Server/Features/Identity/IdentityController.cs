namespace BookHub.Server.Features.Identity
{
    using BookHub.Server.Data.Models;
    using BookHub.Server.Features;
    using BookHub.Server.Features.Identity.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class IdentityController : ApiController
    {
        private readonly IIdentityService identityService;
        private readonly UserManager<User> userManager;
        private readonly AppSettings appSettings;

        public IdentityController(
            IIdentityService identityService,
            UserManager<User> userManager,
            IOptions<AppSettings> appSettings)
        {
            this.identityService = identityService;
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
        }

        [HttpPost(nameof(Register))]
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
                return this.Ok();
            }

            return this.BadRequest();
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordIsValid)
            {
                return this.Unauthorized();
            }

            var token = this.identityService.GenerateJwtToken(this.appSettings.Secret, user.Id, user.UserName!);
            return new LoginResponseModel(token);
        }
    }
}
