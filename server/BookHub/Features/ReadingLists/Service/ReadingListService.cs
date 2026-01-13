namespace BookHub.Features.ReadingLists.Service;

using BookHub.Common;
using BookHub.Data;
using Books.Service.Models;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;
using UserProfile.Service;
using static Common.Utils;
using static Shared.Constants.ErrorMessages;

public class ReadingListService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IProfileService profileService,
    ILogger<ReadingListService> logger) : IReadingListService
{
    public async Task<ResultWith<PaginatedModel<BookServiceModel>>> All(
        string userId,
        ReadingListStatus status,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var readingStatusIsInvalid = !Enum.IsDefined(status);
        if (readingStatusIsInvalid)
        {
            logger.LogWarning(
                "Invalid reading status passed to All(). Status: {Status}",
                status);

            return $"Invalid reading status passed to All(). Status: {status}";
        }

        var books = data
            .ReadingLists
            .AsNoTracking()
            .Where(rl =>
                rl.UserId == userId &&
                rl.Status == status)
            .ToBookServiceModels();

        var total = await books.CountAsync(cancellationToken);
        var items = await books
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new PaginatedModel<BookServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);

        return ResultWith<PaginatedModel<BookServiceModel>>.Success(result);
    }

    public async Task<BookServiceModel?> LastCurrentlyReading(
        string userId,
        CancellationToken cancellationToken = default)
        => await data
            .ReadingLists
            .AsNoTracking()
            .Where(rl => rl.UserId == userId)
            .OrderByDescending(rl => rl.CreatedOn)
            .ToBookServiceModels()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Result> Add(
        ReadingListServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var exists = await data
           .ReadingLists
           .AsNoTracking()
           .AnyAsync(
               rl =>
                   rl.UserId == userId &&
                   rl.BookId == bookId &&
                   rl.Status == readingStatus,
               cancellationToken);

        if (exists)
        {
            return BookAlreadyInTheList;
        }

        var bookIsNotValid = !await this.BookIsValid(bookId, cancellationToken);
        if (bookIsNotValid)
        {
            return "Invalid Book Id!";
        }

        var mapEntity = new ReadingListDbModel
        {
            UserId = userId,
            BookId = bookId,
            Status = readingStatus,
        };

        data.Add(mapEntity);
        await data.SaveChangesAsync(cancellationToken);

        switch (readingStatus)
        {
            case ReadingListStatus.Read:
                await profileService.IncrementReadBooksCount(
                    userId,
                    cancellationToken);

                break;
            case ReadingListStatus.ToRead:
                await profileService.IncrementToReadBooksCount(
                    userId,
                    cancellationToken);
                break;
            case ReadingListStatus.CurrentlyReading:
                await profileService.IncrementCurrentlyReadingBooksCount(
                    userId,
                    cancellationToken);

                break;
            default:
                break;
        }

        return true;
    }

    public async Task<Result> Delete(
        ReadingListServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var mapEntity = await data
            .ReadingLists
            .AsNoTracking()
            .FirstOrDefaultAsync(
                rl =>
                    rl.UserId == userId &&
                    rl.BookId == bookId &&
                    rl.Status == readingStatus,
                cancellationToken);

        if (mapEntity is null) 
        {
            return BookNotInTheList;
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(cancellationToken);

        switch (readingStatus)
        {
            case ReadingListStatus.Read:
                await profileService.DecrementReadBooksCount(
                    userId,
                    cancellationToken);

                break;
            case ReadingListStatus.ToRead:
                await profileService.DecrementToReadBooksCount(
                    userId,
                    cancellationToken);
                break;
            case ReadingListStatus.CurrentlyReading:
                await profileService.DecrementCurrentlyReadingBooksCount(
                    userId,
                    cancellationToken);

                break;
            default:
                break;
        }

        return true;
    }

    private async Task<bool> BookIsValid(
        Guid bookId,
        CancellationToken cancellationToken = default)
        => await data
            .Books
            .AsNoTracking()
            .AnyAsync(
                b => b.Id == bookId,
                cancellationToken);
}
