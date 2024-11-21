namespace BookHub.Server.Features.Review.Votes.Service
{
    public interface IVoteService
    {
        Task<int> VoteAsync(int reviewId, bool isUpvote);
    }
}
