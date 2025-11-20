namespace BookHub.Features.Review.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Common.Constants.ApiRoutes;
    using static Common.Constants.DefaultValues;

    [Authorize]
    public class ReviewController(
        IReviewService service,
        IMapper mapper) : ApiController
    {
        private readonly IReviewService service = service;
        private readonly IMapper mapper = mapper;

        [HttpGet(Id)]
        public async Task<ActionResult<PaginatedModel<ReviewServiceModel>>> AllForBook(
            int id,
            int pageIndex = DefaultPageIndex,
            int pageSize = DefaultPageSize)
            => this.Ok(await this.service.AllForBook(id, pageIndex, pageSize));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var id = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var result = await this.service.Edit(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
