namespace BookHub.Server.Features.Review.Web
{
    using AutoMapper;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class ReviewController(
        IReviewService reviewService,
        ICurrentUserService userService,
        IMapper mapper) : ApiController
    {
        private readonly IReviewService reviewService = reviewService;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateReviewWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateReviewServiceModel>(webModel);
            serviceModel.CreatorId = this.userService.GetId()!;

            var id = await this.reviewService.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), id);
        }
    }
}
