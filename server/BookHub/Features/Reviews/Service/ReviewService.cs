namespace BookHub.Features.Reviews.Service;

using BookHub.Data;
using Common;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.PageClamper;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.ErrorMessages;

public class ReviewService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IProfileService profileService,
    IPageClamper pageClamper,
    ILogger<ReviewService> logger) : IReviewService
{
    private const string ResourceName = "review";

    public async Task<PaginatedModel<ReviewServiceModel>> AllForBook(
        Guid bookId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var reviews = data
           .Reviews
           .AsNoTracking()
           .Where(r => r.BookId == bookId)
           .OrderByDescending(r => r.Votes.Count())
           .ToServiceModels();

        var total = await reviews.CountAsync(cancellationToken);
        var items = await reviews
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<ReviewServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<ReviewServiceModel?> Details(
       Guid id,
       CancellationToken cancellationToken = default)
        => await data
            .Reviews
            .AsNoTracking()
            .ToServiceModels()
            .FirstOrDefaultAsync(
                r => r.Id == id,
                cancellationToken);

    public async Task<ResultWith<ReviewServiceModel>> Create(
        CreateReviewServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var bookId = serviceModel.BookId;
        var userId = userService.GetId()!;

        var bookIdIsInvalid = await this.BookIdIsInvalid(
            bookId,
            cancellationToken);

        if (bookIdIsInvalid)
        {
            return this.LogAndReturnInvalidBookId(bookId, userId);
        }

        var reviewIsDuplicated = await this.UserAlreadyReviewedTheBook(
            userId,
            serviceModel.BookId,
            cancellationToken);

        if (reviewIsDuplicated)
        {
            return this.LogAndReturnDuplication(bookId, userId);
        }

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = userId;

        data.Add(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Review with Id: {id} was created.",
            dbModel.Id);

        await this.CalculateBookRating(
            bookId,
            serviceModel.Rating,
            cancellationToken: cancellationToken);

        await this.CalculateAuthorRating(
            bookId,
            serviceModel.Rating,
            cancellationToken: cancellationToken);

        await profileService.IncrementWrittenReviewsCount(
            userId,
            cancellationToken);

        var result = dbModel.ToServiceModel();
        return ResultWith<ReviewServiceModel>.Success(result);
    }

    public async Task<Result> Edit(
        Guid reviewId,
        CreateReviewServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            reviewId,
            cancellationToken);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFound(reviewId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;

        if (isNotCreator)
        {
            return LogAndReturnForbidden(reviewId, userId);
        }

        var oldRating = dbModel.Rating;
        var bookId = dbModel.BookId;

        if (serviceModel.BookId != bookId)
        {
            logger.LogWarning(
                "User attempted to change review book from {OldBookId} to {NewBookId} for review {ReviewId}",
                bookId,
                serviceModel.BookId,
                reviewId);

            return BookIdCannotChange;
        }

        serviceModel.UpdateDbModel(dbModel);

        await data.SaveChangesAsync(cancellationToken);

        await this.CalculateBookRating(
            bookId,
            dbModel.Rating,
            oldRating,
            cancellationToken: cancellationToken);

        await this.CalculateAuthorRating(
            bookId,
            dbModel.Rating,
            oldRating,
            cancellationToken: cancellationToken);

        return true;
    }

    public async Task<Result> Delete(
        Guid reviewId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            reviewId,
            cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFound(reviewId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnForbidden(reviewId, userId);
        }

        var oldRating = dbModel.Rating;
        var bookId = dbModel.BookId;

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        await this.CalculateBookRating(
            bookId,
            0,
            oldRating,
            true,
            cancellationToken);

        await this.CalculateAuthorRating(
            bookId,
            0,
            oldRating,
            true,
            cancellationToken);

        return true;
    }

    private async Task<ReviewDbModel?> GetDbModel(
        Guid reviewId,
        CancellationToken cancellationToken = default)
        => await data
            .Reviews
            .FindAsync([reviewId], cancellationToken);

    private Result LogAndReturnNotFound(Guid reviewId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(ReviewDbModel),
            reviewId);

        return Result.NotFound(string.Format(
            ResourceNotFound,
            ResourceName));
    }

    private Result LogAndReturnForbidden(
        Guid reviewId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(ReviewDbModel),
            reviewId);

        return Result.Forbidden(string.Format(
            ResourceForbidden,
            ResourceName));
    }

    private ResultWith<ReviewServiceModel> LogAndReturnDuplication(
        Guid bookId,
        string userId)
    {
        logger.LogWarning(
            "User with Id: {UserId} already wrote review for book with Id: {BookId}",
            userId,
            bookId);

        return ResultWith<ReviewServiceModel>.Conflict(AlreadyReviewed);
    }

    private ResultWith<ReviewServiceModel> LogAndReturnInvalidBookId(
        Guid bookId,
        string userId)
    {
        logger.LogWarning(
            "User with Id: {UserId} attempted to create review for invalid book Id: {BookId}",
            userId,
            bookId);

        return BookDoesNotExist;
    }

    private async Task<bool> BookIdIsInvalid(
        Guid bookId,
        CancellationToken cancellationToken = default)
        => !await data
            .Books
            .AsNoTracking()
            .AnyAsync(
                b => b.Id == bookId,
                cancellationToken);

    private async Task<bool> UserAlreadyReviewedTheBook(
        string userId,
        Guid bookId,
        CancellationToken cancellationToken = default)
        => await data
            .Reviews
            .AsNoTracking()
            .AnyAsync(
                r => r.CreatorId == userId && r.BookId == bookId,
                cancellationToken);

    private async Task CalculateBookRating(
        Guid bookId,
        int newRating,
        int? oldRating = null,
        bool isDeleteMode = false,
        CancellationToken cancellationToken = default)
    {
        var book = await data
           .Books
           .FindAsync([bookId], cancellationToken);

        if (book is null) 
        {
            return;
        }

        var newRatingsCount = isDeleteMode
            ? Math.Max(0, book.RatingsCount - 1)
            : book.RatingsCount;

        if (oldRating.HasValue && newRatingsCount == 0)
        {
            book.AverageRating = 0;
            book.RatingsCount = 0;

            await data.SaveChangesAsync(cancellationToken);

            return;
        }

        double newAverageRating;

        if (oldRating.HasValue)
        {
            newAverageRating =
                ((book.AverageRating * book.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
        }
        else
        {
            newRatingsCount++;
            newAverageRating =
                ((book.AverageRating * book.RatingsCount) + newRating) / newRatingsCount;
        }

        book.AverageRating = newRatingsCount == 0 
            ? 0
            : newAverageRating;

        book.RatingsCount = newRatingsCount;

        await data.SaveChangesAsync(cancellationToken);

    }

    private async Task CalculateAuthorRating(
        Guid bookId,
        int newRating,
        int? oldRating = null,
        bool isDeleteMode = false,
        CancellationToken cancellationToken = default)
    {
        var authorId = await data
           .Books
           .Where(b => b.Id == bookId)
           .Select(b => b.AuthorId)
           .FirstOrDefaultAsync(cancellationToken);

        var author = await data
           .Authors
           .FindAsync([authorId], cancellationToken);

        if (author is null) 
        {
            return;
        }

        var newRatingsCount = isDeleteMode
            ? Math.Max(0, author.RatingsCount - 1)
            : author.RatingsCount;

        if (oldRating.HasValue && newRatingsCount == 0)
        {
            author.AverageRating = 0;
            author.RatingsCount = 0;

            await data.SaveChangesAsync(cancellationToken);

            return;
        }

        double newAverageRating;
        if (oldRating.HasValue)
        {
            newAverageRating =
                ((author.AverageRating * author.RatingsCount) - oldRating.Value + newRating) / newRatingsCount;
        }
        else
        {
            newRatingsCount++;
            newAverageRating =
                ((author.AverageRating * author.RatingsCount) + newRating) / newRatingsCount;
        }

        author.AverageRating = newRatingsCount == 0 ? 0 : newAverageRating;
        author.RatingsCount = newRatingsCount;

        await data.SaveChangesAsync(cancellationToken);
    }
}
