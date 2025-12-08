namespace BookHub.Features.Review.Web;

using BookHub.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Service;

[Authorize]
public class VoteController(IVoteService service) : ApiController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        VoteRequestModel model,
        CancellationToken token = default) 
    {
        await service.Create(
            model.ReviewId,
            model.IsUpvote,
            token);

        return this.Ok(model.ReviewId);
    }
}
