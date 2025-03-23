namespace BookHub.Features.Review.Web
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
            var voteId = await this.service.Create(model.ReviewId, model.IsUpvote);

            return voteId == null
                ? this.BadRequest()
                : this.Created(nameof(this.Create), model.ReviewId);
        }
    }
}
