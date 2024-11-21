namespace BookHub.Server.Features.Review.Votes.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;

    [Authorize]
    public class VoteController(IVoteService service) : ApiController
    {
        private readonly IVoteService service = service;

        [HttpPost]
        public async Task<ActionResult<int>> Create(VoteRequestModel model)
        {
            var reviewId = await this.service.VoteAsync(model.ReviewId, model.IsUpvote);

            return this.Created(nameof(this.Create), reviewId);
        }
    }
}
