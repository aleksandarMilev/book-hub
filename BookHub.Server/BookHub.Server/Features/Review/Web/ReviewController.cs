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
        IReviewService reviewService,
        IMapper mapper) : ApiController
    {
        private readonly IReviewService reviewService = reviewService;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var id = await this.reviewService.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            var result = await this.reviewService.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }
    }
}
