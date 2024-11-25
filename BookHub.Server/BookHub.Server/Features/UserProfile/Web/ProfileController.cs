namespace BookHub.Server.Features.UserProfile.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class ProfileController(
        IProfileService service,
        IMapper mapper) : ApiController
    {
        private readonly IProfileService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<ProfileServiceModel>> Get()
            => this.Ok(await this.service.GetAsync());

        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateProfileWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateProfileServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(CreateProfileWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateProfileServiceModel>(webModel);
            var result = await this.service.EditAsync(serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var result = await this.service.DeleteAsync();

            return this.NoContentOrBadRequest(result);
        }
    }
}
