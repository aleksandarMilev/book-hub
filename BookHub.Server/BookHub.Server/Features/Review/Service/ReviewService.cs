namespace BookHub.Server.Features.Review.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
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

        public async Task<PaginatedModel<ReviewServiceModel>> AllForBookAsync(int bookId, int pageIndex, int pageSize)
        {
            var reviews = this.data
               .Reviews
               .Where(r => r.BookId == bookId)
               .ProjectTo<ReviewServiceModel>(this.mapper.ConfigurationProvider);

            var totalReviews = await reviews.CountAsync();

            var paginatedReviews = await reviews
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<ReviewServiceModel>(paginatedReviews, totalReviews, pageIndex, pageSize);
        }

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

            await this.CalculateBookRatingAsync(model.BookId, model.Rating);
            await this.CalculateAuthorRatingAsync(model.BookId, model.Rating);

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

            await this.CalculateBookRatingAsync(model.BookId, model.Rating, review.Rating);
            await this.CalculateAuthorRatingAsync(model.BookId, model.Rating, review.Rating);

            this.mapper.Map(model, review);

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
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
                return UnauthorizedReviewDelete;
            }

            await this.CalculateBookRatingAsync(review.BookId, 0, review.Rating, true);
            await this.CalculateAuthorRatingAsync(review.BookId, 0, review.Rating, true);

            this.data.Remove(review);
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

        private async Task CalculateBookRatingAsync(int bookId, int newRating, int? oldRating = null, bool isDeleteMode = false)
        {
            var book = await this.data
               .Books
               .FindAsync(bookId)
               ?? throw new InvalidOperationException("Book not found!");

            double newAverageRating;
            var newRatingsCount = isDeleteMode ? --book.RatingsCount : book.RatingsCount;

            if (oldRating.HasValue)
            {
                newAverageRating = ((book.AverageRating * book.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
            }
            else 
            {
                newRatingsCount = book.RatingsCount + 1;
                newAverageRating = ((book.AverageRating * book.RatingsCount) + newRating) / newRatingsCount;
            }

            if (newRatingsCount == 0)
            {
                newAverageRating = 0;
            }

            book.AverageRating = newAverageRating;
            book.RatingsCount = newRatingsCount;
        }

        private async Task CalculateAuthorRatingAsync(int bookId, int newRating, int? oldRating = null, bool isDeleteMode = false)
        {
            var authorId = await this.data
               .Books
               .Where(b => b.Id == bookId)
               .Select(b => b.AuthorId)
               .FirstOrDefaultAsync();

            var author = await this.data
               .Authors
               .FindAsync(authorId)
               ?? throw new InvalidOperationException("Author not found!");

            double newAverageRating;
            var newRatingsCount = isDeleteMode ? --author.RatingsCount : author.RatingsCount;

            if (oldRating.HasValue)
            {
                newAverageRating = ((author.AverageRating * author.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
            }
            else 
            {
                newRatingsCount = author.RatingsCount + 1;
                newAverageRating = ((author.AverageRating * author.RatingsCount) + newRating) / newRatingsCount;
            }

            if (newRatingsCount == 0)
            {
                newAverageRating = 0; 
            }

            author.AverageRating = newAverageRating;
            author.RatingsCount = newRatingsCount;
        }
    }
}
