namespace BookHub.Features.Review.Service;

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
        CancellationToken token = default)
    {
        var reviewExists = await data
            .Reviews
            .AnyAsync(
                r => r.Id == reviewId,
                token);

        if (!reviewExists)
        {
            return null;
        }

        var userId = userService.GetId()!;
        var voteExists = await data
            .Votes
            .AnyAsync(
                v => 
                    v.ReviewId == reviewId &&
                    v.IsUpvote == isUpvote &&
                    v.CreatorId == userId,
                token);

        if (voteExists)
        {
            return null;
        }

        var oppositeVote = await data
            .Votes
            .FirstOrDefaultAsync(
                v =>
                    v.ReviewId == reviewId &&
                    v.IsUpvote == !isUpvote &&
                    v.CreatorId == userId,
                token);

        if (oppositeVote != null)
        {
            data.Remove(oppositeVote);
        }

        var vote = new VoteDbModel
        { 
            ReviewId = reviewId,
            IsUpvote = isUpvote,
            CreatorId = userId
        };

        data.Add(vote);
        await data.SaveChangesAsync(token);

        return vote.Id;
    }
}
