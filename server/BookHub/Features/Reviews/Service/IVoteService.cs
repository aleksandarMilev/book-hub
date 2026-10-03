namespace BookHub.Features.Reviews.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;

public interface IVoteService : ITransientService
{
    // Toggles the caller's vote. Returns the review ID, or NotFound when the review doesn't exist.
    Task<ResultWith<Guid>> Create(
        Guid reviewId,
        bool isUpvote,
        CancellationToken cancellationToken = default);
}
