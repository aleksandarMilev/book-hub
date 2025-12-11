namespace BookHub.Features.Review.Service;

using Infrastructure.Services.ServiceLifetimes;

public interface IVoteService : ITransientService
{
    Task<int?> Create(
        Guid reviewId,
        bool isUpvote,
        CancellationToken token = default);
}
