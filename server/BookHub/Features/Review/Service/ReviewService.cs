namespace BookHub.Features.Review.Service;

using BookHub.Common;
using BookHub.Data;
using Data.Models;
using Features.UserProfile.Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Common.Constants.DefaultValues;

public class ReviewService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IProfileService profileService,
    ILogger<ReviewService> logger) : IReviewService
{
    public async Task<PaginatedModel<ReviewServiceModel>> AllForBook(
        Guid bookId,
        int pageIndex,
        int pageSize,
        CancellationToken token = default)
    {
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? DefaultPageSize : pageSize;

        var reviews = data
           .Reviews
           .Where(r => r.BookId == bookId)
           .OrderByDescending(r => r.Votes.Count())
           .ToServiceModels();

        var totalReviews = await reviews.CountAsync(token);
        var paginatedReviews = await reviews
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<ReviewServiceModel>(
            paginatedReviews,
            totalReviews,
            pageIndex,
            pageSize);
    }

    public async Task<ReviewServiceModel?> Details(
       Guid id,
       CancellationToken token = default)
        => await data
            .Reviews
            .ToServiceModels()
            .FirstOrDefaultAsync(r => r.Id == id, token);

    public async Task<ResultWith<ReviewServiceModel>> Create(
        CreateReviewServiceModel serviceModel,
        CancellationToken token = default)
    {
        var bookId = serviceModel.BookId;
        var userId = userService.GetId()!;

        var bookIdIsInvalid = await this.BookIdIsInvalid(bookId, token);
        if (bookIdIsInvalid)
        {
            return this.LogAndReturnInvalidBookIdMessage(bookId, userId);
        }

        var reviewIsDuplicated = await this.UserAlreadyReviewedTheBook(
            userId,
            serviceModel.BookId,
            token);

        if (reviewIsDuplicated)
        {
            return this.LogAndReturnDuplicationMessage(bookId, userId);
        }

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = userId;

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Review with Id: {id} was created.",
            dbModel.Id);

        await this.CalculateBookRating(
            bookId,
            serviceModel.Rating,
            token: token);

        await this.CalculateAuthorRating(
            bookId,
            serviceModel.Rating,
            token: token);

        await profileService.UpdateCount(
            userId,
            nameof(UserProfile.ReviewsCount),
            x => ++x);

        var result = dbModel.ToServiceModel();
        return ResultWith<ReviewServiceModel>.Success(result);
    }

    public async Task<Result> Edit(
        Guid id,
        CreateReviewServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;

        if (isNotCreator)
        {
            return LogAndReturnUnauthorizedMessage(id, userId);
        }

        var oldRating = dbModel.Rating;
        serviceModel.UpdateDbModel(dbModel);

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Review with Id: {id} was update.",
            dbModel.Id);

        await this.CalculateBookRating(
            serviceModel.BookId,
            serviceModel.Rating,
            oldRating,
            token: token);

        await this.CalculateAuthorRating(
            serviceModel.BookId,
            serviceModel.Rating,
            oldRating,
            token: token);

        return true;
    }

    public async Task<Result> Delete(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(id, userId);
        }

        var oldRating = dbModel.Rating;
        var bookId = dbModel.BookId;

        data.Remove(dbModel);
        await data.SaveChangesAsync(token);

        await this.CalculateBookRating(
            bookId,
            0,
            oldRating,
            true,
            token);

        await this.CalculateAuthorRating(
            bookId,
            0,
            oldRating,
            true,
            token);

        return true;
    }

    private async Task<ReviewDbModel?> GetDbModel(
        Guid id,
        CancellationToken token = default)
        => await data
            .Reviews
            .FindAsync([id], token);

    private string LogAndReturnNotFoundMessage(Guid id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(ReviewDbModel),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(ReviewDbModel),
            id);
    }

    private string LogAndReturnUnauthorizedMessage(
        Guid reviewId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(ReviewDbModel),
            reviewId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(ReviewDbModel),
            reviewId);
    }

    private string LogAndReturnDuplicationMessage(
        Guid bookId,
        string userId)
    {
        logger.LogWarning(
            "User with Id: {UserId} already wrote review for book with Id: {BookId}",
            userId,
            bookId);

        return string.Format(
            "User with Id: {0} already wrote review for book with Id: {1}",
            userId,
            bookId);
    }

    private string LogAndReturnInvalidBookIdMessage(
        Guid bookId,
        string userId)
    {
        logger.LogWarning(
            "User with Id: {UserId} attempted to create review for invalid book Id: {BookId}",
            userId,
            bookId);

        return string.Format(
            "User with Id: {0} attempted to create review for invalid book Id: {1}",
            userId,
            bookId);
    }

    private async Task<bool> BookIdIsInvalid(
        Guid bookId,
        CancellationToken token = default)
    {
        var id = await data
           .Books
           .Where(b => b.Id == bookId)
           .Select(b => b.Id)
           .FirstOrDefaultAsync(token);

        return id == Guid.Empty;
    }

    private async Task<bool> UserAlreadyReviewedTheBook(
        string userId,
        Guid bookId,
        CancellationToken token = default)
        => await data
            .Reviews
            .AnyAsync(
                r => r.CreatorId == userId && r.BookId == bookId,
                token);

    private async Task CalculateBookRating(
        Guid bookId,
        int newRating,
        int? oldRating = null,
        bool isDeleteMode = false,
        CancellationToken token = default)
    {
        var book = await data
           .Books
           .FindAsync([bookId], token);

        if (book is null) 
        {
            return;
        }

        double newAverageRating;
        var newRatingsCount = isDeleteMode
            ? Math.Max(0, book.RatingsCount - 1)
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

        await data.SaveChangesAsync(token);
    }

    private async Task CalculateAuthorRating(
        Guid bookId,
        int newRating,
        int? oldRating = null,
        bool isDeleteMode = false,
        CancellationToken token = default)
    {
        var authorId = await data
           .Books
           .Where(b => b.Id == bookId)
           .Select(b => b.AuthorId)
           .FirstOrDefaultAsync(token);

        var author = await data
           .Authors
           .FindAsync([authorId], token);

        if (author is null) 
        {
            return;
        }

        double newAverageRating;
        var newRatingsCount = isDeleteMode
            ? Math.Max(0, author.RatingsCount - 1)
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

        await data.SaveChangesAsync(token);
    }
}
