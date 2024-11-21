namespace BookHub.Server.Features.Review.Service
{
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;

    public class VoteService(
        BookHubDbContext data,
        ICurrentUserService userService) : IVoteService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;

        public async Task<int?> CreateAsync(int reviewId, bool isUpvote)
        {
            var userId = this.userService.GetId()!;

            var voteExists = await this.data
                .Votes
                .AnyAsync(v => 
                    v.ReviewId == reviewId &&
                    v.IsUpvote == isUpvote &&
                    v.CreatorId == userId);

            if (voteExists)
            {
                return null;
            }

            var oppositeVote = await this.data
                .Votes
                .FirstOrDefaultAsync(v =>
                    v.ReviewId == reviewId &&
                    v.IsUpvote == !isUpvote &&
                    v.CreatorId == userId);

            if (oppositeVote != null)
            {
                this.data.Remove(oppositeVote);
            }

            var vote = new Vote()
            { 
                ReviewId = reviewId,
                IsUpvote = isUpvote,
                CreatorId = userId
            };

            this.data.Add(vote);
            await this.data.SaveChangesAsync();

            return vote.Id;
        }
    }
}
