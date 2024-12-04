namespace BookHub.Server.Features.Review.Service
{
    using Infrastructure.Services.ServiceLifetimes;

    public interface IVoteService : ITransientService
    {
        Task<int?> CreateAsync(int reviewId, bool isUpvote);
    }
}
