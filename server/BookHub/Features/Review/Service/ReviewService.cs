namespace BookHub.Features.Review.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Data;
    using Common;
    using Data.Models;
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.UserProfile.Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using UserProfile.Service;

    using static Common.ErrorMessage;

    public class ReviewService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IProfileService profileService,
        IMapper mapper) : IReviewService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IProfileService profileService = profileService;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedModel<ReviewServiceModel>> AllForBook(
            int bookId,
            int pageIndex,
            int pageSize)
        {
            var reviews = this.data
               .Reviews
               .Where(r => r.BookId == bookId)
               .OrderByDescending(r => r.Votes.Count())
               .ProjectTo<ReviewServiceModel>(this.mapper.ConfigurationProvider);

            var totalReviews = await reviews.CountAsync();
            var paginatedReviews = await reviews
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<ReviewServiceModel>(
                paginatedReviews,
                totalReviews,
                pageIndex,
                pageSize);
        }

        public async Task<int> Create(CreateReviewServiceModel model)
        {
            var userId = this.userService.GetId()!;

            if (await this.UserAlreadyReviewedTheBook(userId, model.BookId))
            {
                throw new ReviewDuplicatedException(
                    this.userService.GetUsername()!,
                    model.BookId);
            }

            await this.ValidateBookId(model.BookId);

            var review = this.mapper.Map<Review>(model);
            review.CreatorId = userId;

            this.data.Add(review);
            await this.data.SaveChangesAsync();

            await this.CalculateBookRating(model.BookId, model.Rating);
            await this.CalculateAuthorRating(model.BookId, model.Rating);

            await this.profileService.UpdateCount(
                userId,
                nameof(UserProfile.ReviewsCount),
                x => ++x);

            return review.Id;
        }

        public async Task<Result> Edit(int id, CreateReviewServiceModel model)
        {
            var review = await this.data
               .Reviews
               .FindAsync(id);

            if (review is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Review), id);
            }

            var userId = this.userService.GetId()!;

            if (review.CreatorId != userId)
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Review),
                    id);
            }

            var oldRating = review.Rating;

            this.mapper.Map(model, review);

            await this.data.SaveChangesAsync();

            await this.CalculateBookRating(
                model.BookId,
                model.Rating,
                oldRating);

            await this.CalculateAuthorRating(
                model.BookId,
                model.Rating,
                oldRating);

            return true;
        }


        public async Task<Result> Delete(int id)
        {
            var review = await this.data
                 .Reviews
                 .FindAsync(id);

            if (review is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Review),
                    id);
            }

            var userId = this.userService.GetId()!;

            var userIsNotCreator = review.CreatorId != userId;
            var userIsNotAdmin = !this.userService.IsAdmin();

            if (userIsNotCreator && userIsNotAdmin)
            {
                return string.Format(
                     UnauthorizedDbEntityAction,
                     this.userService.GetUsername(),
                     nameof(Review),
                     id);
            }

            var oldRating = review.Rating;
            var bookId = review.BookId;

            this.data.Remove(review);
            await this.data.SaveChangesAsync();

            await this.CalculateBookRating(
                bookId,
                0,
                oldRating,
                true);

            await this.CalculateAuthorRating(
                bookId,
                0,
                oldRating,
                true);

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
                throw new DbEntityNotFoundException<int>(nameof(Book), bookId);
            }
        }

        private async Task<bool> UserAlreadyReviewedTheBook(
            string userId,
            int bookId)
            => await this.data
                .Reviews
                .AnyAsync(r => 
                    r.CreatorId == userId && 
                    r.BookId == bookId);

        private async Task CalculateBookRating(
            int bookId,
            int newRating,
            int? oldRating = null,
            bool isDeleteMode = false)
        {
            var book = await this.data
               .Books
               .FindAsync(bookId)
               ?? throw new DbEntityNotFoundException<int>(nameof(Book), bookId);

            double newAverageRating;
            var newRatingsCount = isDeleteMode 
                ? book.RatingsCount - 1
                : book.RatingsCount;

            if (oldRating.HasValue)
            {
                newAverageRating = ((book.AverageRating * book.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
            }
            else
            {
                newRatingsCount++;
                newAverageRating = ((book.AverageRating * book.RatingsCount) + newRating) / newRatingsCount;
            }

            if (newRatingsCount == 0)
            {
                newAverageRating = 0;
            }

            book.AverageRating = newAverageRating;
            book.RatingsCount = newRatingsCount;

            await this.data.SaveChangesAsync();
        }

        private async Task CalculateAuthorRating(
            int bookId,
            int newRating,
            int? oldRating = null,
            bool isDeleteMode = false)
        {
            var authorId = await this.data
               .Books
               .Where(b => b.Id == bookId)
               .Select(b => b.AuthorId)
               .FirstOrDefaultAsync();

            var author = await this.data
               .Authors
               .FindAsync(authorId)
               ?? throw new DbEntityNotFoundException<int?>(nameof(Author), authorId);

            double newAverageRating;
            var newRatingsCount = isDeleteMode
                ? author.RatingsCount - 1
                : author.RatingsCount;

            if (oldRating.HasValue)
            {
                newAverageRating = ((author.AverageRating * author.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
            }
            else
            {
                newRatingsCount++;
                newAverageRating = ((author.AverageRating * author.RatingsCount) + newRating) / newRatingsCount;
            }

            if (newRatingsCount == 0)
            {
                newAverageRating = 0; 
            }

            author.AverageRating = newAverageRating;
            author.RatingsCount = newRatingsCount;

            await this.data.SaveChangesAsync();
        }
    }
}
