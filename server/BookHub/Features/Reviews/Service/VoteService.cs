namespace BookHub.Features.Reviews.Service;

using BookHub.Data;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

public class VoteService(
    BookHubDbContext data,
    ICurrentUserService userService) : IVoteService
{
    public async Task<int?> Create(
        Guid reviewId,
        bool isUpvote,
        CancellationToken cancellationToken = default)
    {
        var reviewExists = await data
            .Reviews
            .AsNoTracking()
            .AnyAsync(
                r => r.Id == reviewId,
                cancellationToken);

        if (!reviewExists)
        {
            return null;
        }

        var userId = userService.GetId()!;

        var existingVote = await data.Votes
            .FirstOrDefaultAsync(
                v =>
                    v.ReviewId == reviewId &&
                    v.CreatorId == userId,
                cancellationToken);

        if (existingVote is not null)
        {
            if (existingVote.IsUpvote == isUpvote)
            {
                data.Remove(existingVote);

                await data.SaveChangesAsync(cancellationToken);

                return null;
            }

            existingVote.IsUpvote = isUpvote;

            await data.SaveChangesAsync(cancellationToken);

            return existingVote.Id;
        }

        var vote = new VoteDbModel
        {
            ReviewId = reviewId,
            IsUpvote = isUpvote,
            CreatorId = userId
        };

        data.Add(vote);

        await data.SaveChangesAsync(cancellationToken);

        return vote.Id;
    }
}
