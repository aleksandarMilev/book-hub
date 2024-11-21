namespace BookHub.Server.Features.Review.Votes.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class VoteService(BookHubDbContext data) : IVoteService
    {
        private readonly BookHubDbContext data = data;

        public async Task<int> VoteAsync(int id, bool isUpvote)
        {
            var review = await this.data
                .Reviews
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new InvalidOperationException("Review not found!");

            if (isUpvote)
            {
                review.Upvotes++;
            }
            else
            {
                review.Downvotes++;
            }

            await this.data.SaveChangesAsync();

            return id;
        }
    }
}
