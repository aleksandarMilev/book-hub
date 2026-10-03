namespace BookHub.Features.Reviews.Service;

using BookHub.Data;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;

using static Common.Constants.ErrorMessages;

public class VoteService(
    BookHubDbContext data,
    ICurrentUserService userService) : IVoteService
{
    private const string ResourceName = "review";

    public async Task<ResultWith<Guid>> Create(
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
            return ResultWith<Guid>.NotFound(string.Format(
                ResourceNotFound,
                ResourceName));
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
            }
            else
            {
                existingVote.IsUpvote = isUpvote;
            }

            await data.SaveChangesAsync(cancellationToken);

            return ResultWith<Guid>.Success(reviewId);
        }

        var vote = new VoteDbModel
        {
            ReviewId = reviewId,
            IsUpvote = isUpvote,
            CreatorId = userId
        };

        data.Add(vote);

        await data.SaveChangesAsync(cancellationToken);

        return ResultWith<Guid>.Success(reviewId);
    }
}
