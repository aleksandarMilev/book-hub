namespace BookHub.Server.Features.Review.Service
{
    public interface IVoteService
    {
        Task<int?> CreateAsync(int reviewId, bool isUpvote);
    }
}
