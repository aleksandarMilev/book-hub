namespace BookHub.Features.Reviews.Web.Models;

public class VoteRequestModel
{
    public Guid ReviewId { get; init; }

    public bool IsUpvote { get; init; }
}
