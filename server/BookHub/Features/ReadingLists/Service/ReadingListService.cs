namespace BookHub.Features.ReadingLists.Service;

using BookHub.Data;
using Books.Service.Models;
using Common;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.PageClamper;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;
using UserProfile.Service;

using static Shared.Constants.ErrorMessages;

public class ReadingListService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IProfileService profileService,
    IPageClamper pageClamper,
    ILogger<ReadingListService> logger) : IReadingListService
{
    public async Task<ResultWith<PaginatedModel<BookServiceModel>>> All(
        string userId,
        ReadingListStatus status,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
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
            .Where(rl => 
                rl.UserId == userId && 
                rl.Status == ReadingListStatus.CurrentlyReading)
            .OrderByDescending(rl => rl.ModifiedOn ?? rl.CreatedOn)
            .ToBookServiceModels()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Result> Add(
        ReadingListServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var bookIsNotValid = !await this.BookIsValid(
            bookId,
            cancellationToken);

        if (bookIsNotValid)
        {
            return "Invalid Book Id!";
        }

        var existing = await data
            .ReadingLists
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                rl => rl.UserId == userId && rl.BookId == bookId,
                cancellationToken);

        if (existing is null)
        {
            var mapEntity = new ReadingListDbModel
            {
                UserId = userId,
                BookId = bookId,
                Status = readingStatus,
                CompletedOn = readingStatus == ReadingListStatus.Read
                    ? DateTime.UtcNow
                    : null,
            };

            data.Add(mapEntity);
            await data.SaveChangesAsync(cancellationToken);

            await IncrementProfileCounter(
                userId,
                readingStatus,
                cancellationToken);

            return true;
        }
        else if (existing.IsDeleted)
        {
            existing.IsDeleted = false;
            existing.DeletedOn = null;
            existing.DeletedBy = null;

            existing.Status = readingStatus;
            existing.CompletedOn = readingStatus == ReadingListStatus.Read
                ? (existing.CompletedOn ?? DateTime.UtcNow)
                : null;

            await data.SaveChangesAsync(cancellationToken);

            await IncrementProfileCounter(
                userId,
                readingStatus,
                cancellationToken);

            return true;
        }

        if (existing.Status == readingStatus)
        {
            return BookAlreadyInTheList;
        }

        var oldStatus = existing.Status;
        existing.Status = readingStatus;

        if (readingStatus == ReadingListStatus.Read)
        {
            existing.CompletedOn ??= DateTime.UtcNow;
        }
        else
        {
            existing.CompletedOn = null;
        }

        await data.SaveChangesAsync(cancellationToken);

        await this.DecrementProfileCounter(
            userId,
            oldStatus,
            cancellationToken);

        await this.IncrementProfileCounter(
            userId,
            readingStatus,
            cancellationToken);

        return true;
    }

    public async Task<Result> Delete(
        ReadingListServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var bookId = serviceModel.BookId;

        var mapEntity = await data
            .ReadingLists
            .FirstOrDefaultAsync(
                rl => rl.UserId == userId && rl.BookId == bookId,
                cancellationToken);

        if (mapEntity is null)
        {
            return BookNotInTheList;
        }

        var oldStatus = mapEntity.Status;

        data.Remove(mapEntity);
        await data.SaveChangesAsync(cancellationToken);

        await this.DecrementProfileCounter(
            userId,
            oldStatus,
            cancellationToken);

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

    private Task IncrementProfileCounter(
        string userId,
        ReadingListStatus status,
        CancellationToken cancellationToken)
        => status switch
        {
            ReadingListStatus.Read => profileService.IncrementReadBooksCount(
                userId,
                cancellationToken),
            ReadingListStatus.ToRead => profileService.IncrementToReadBooksCount(
                userId,
                cancellationToken),
            ReadingListStatus.CurrentlyReading => profileService.IncrementCurrentlyReadingBooksCount(
                userId,
                cancellationToken),
            _ => Task.CompletedTask
        };

    private Task DecrementProfileCounter(
        string userId,
        ReadingListStatus status,
        CancellationToken cancellationToken)
        => status switch
        {
            ReadingListStatus.Read => profileService.DecrementReadBooksCount(
                userId,
                cancellationToken),
            ReadingListStatus.ToRead => profileService.DecrementToReadBooksCount(
                userId,
                cancellationToken),
            ReadingListStatus.CurrentlyReading => profileService.DecrementCurrentlyReadingBooksCount(
                userId,
                cancellationToken),
            _ => Task.CompletedTask
        };
}
