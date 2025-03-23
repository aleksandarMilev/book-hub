namespace BookHub.Features.Review.Service
{
    using Infrastructure.Services.ServiceLifetimes;

    public interface IVoteService : ITransientService
    {
        Task<int?> Create(int reviewId, bool isUpvote);
    }
}
