namespace BookHub.Server.Features.Review.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class ReviewController(
        IReviewService service,
        IMapper mapper) : ApiController
    {
        private readonly IReviewService service = service;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var id = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
