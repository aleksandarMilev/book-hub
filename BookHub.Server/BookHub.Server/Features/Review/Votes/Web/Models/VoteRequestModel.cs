namespace BookHub.Server.Features.Review.Votes.Web.Models
{
    public class VoteRequestModel
    {
        public int ReviewId { get; init; }

        public bool IsUpvote { get; init; }
    }
}
