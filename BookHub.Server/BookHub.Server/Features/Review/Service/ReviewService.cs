namespace BookHub.Server.Features.Review.Service
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Messages.Error.Review;

    public class ReviewService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : IReviewService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<int> CreateAsync(CreateReviewServiceModel model)
        {
            var userId = this.userService.GetId()!;

            if (await this.UserAlreadyReviewedTheBookAsync(userId, model.BookId))
            {
                throw new InvalidOperationException("This user has already written a review!");
            }

            await this.ValidateBookId(model.BookId);

            var review = this.mapper.Map<Review>(model);
            review.CreatorId = userId;

            this.data.Add(review);
            await this.data.SaveChangesAsync();

            return review.Id;
        }

        public async Task<Result> EditAsync(int id, CreateReviewServiceModel model)
        {
            var review = await this.data
               .Reviews
               .FindAsync(id);

            if (review is null)
            {
                return ReviewNotFound;
            }

            var userId = this.userService.GetId()!;

            if (review.CreatorId != userId)
            {
                return UnauthorizedReviewEdit;
            }

            this.mapper.Map(model, review);

            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task ValidateBookId(int bookId)
        {
            var id = await this.data
               .Books
               .Where(b => b.Id == bookId)
               .Select(b => b.Id)
               .FirstOrDefaultAsync();

            if (id == 0)
            {
                throw new InvalidOperationException("Book not found!");
            }
        }

        private async Task<bool> UserAlreadyReviewedTheBookAsync(string userId, int bookId)
            => await this.data
                .Reviews
                .AnyAsync(r => r.CreatorId == userId && r.BookId == bookId);
    }
}
