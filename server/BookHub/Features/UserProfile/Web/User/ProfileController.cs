namespace BookHub.Features.UserProfile.Web.User
{
    using AutoMapper;
    using BookHub.Common;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Common.Constants.ApiRoutes;

    [Authorize]
    public class ProfileController(
        IProfileService service,
        IMapper mapper) : ApiController
    {
        private readonly IProfileService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Top)]
        public async Task<ActionResult<IEnumerable<ProfileServiceModel>>> TopThree()
            => this.Ok(await this.service.TopThree());

        [HttpGet(ApiRoutes.Mine)]
        public async Task<ActionResult<ProfileServiceModel>> Mine()
            => this.Ok(await this.service.Mine());

        [HttpGet(Id)]
        public async Task<ActionResult<IProfileServiceModel>> OtherUser(string id)
            => this.Ok(await this.service.OtherUser(id));

        [HttpGet(ApiRoutes.Exists)]
        public async Task<ActionResult<bool>> Exists()
            => this.Ok(await this.service.HasProfile());

        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateProfileWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateProfileServiceModel>(webModel);
            var id = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(CreateProfileWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateProfileServiceModel>(webModel);
            var result = await this.service.Edit(serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var result = await this.service.Delete();

            return this.NoContentOrBadRequest(result);
        }
    }
}
