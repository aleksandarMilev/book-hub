namespace BookHub.Features.ReadingList.Service;

using Book.Service.Models;
using BookHub.Common;
using BookHub.Data;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;
using UserProfile.Data.Models;
using UserProfile.Service;

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
        CancellationToken token = default)
    {
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
            .Where(rl =>
                rl.UserId == userId &&
                rl.Status == status)
            .ToBookServiceModels();

        var total = await books.CountAsync(token);
        var paginatedBooks = await books
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        var result = new PaginatedModel<BookServiceModel>(
            paginatedBooks,
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
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var exists = await data
           .ReadingLists
           .AnyAsync(
               rl =>
                   rl.UserId == userId &&
                   rl.BookId == bookId &&
                   rl.Status == readingStatus,
               token);

        if (exists)
        {
            return BookAlreadyInTheList;
        }

        var bookIsNotValid = !await this.BookIsValid(bookId, token);
        if (bookIsNotValid)
        {
            return "Invalid Book Id!";
        }

        var mapEntity = new ReadingList
        {
            UserId = userId,
            BookId = bookId,
            Status = readingStatus,
        };

        data.Add(mapEntity);
        await data.SaveChangesAsync(token);

        switch (readingStatus)
        {
            case ReadingListStatus.Read:
                await profileService.IncrementReadBooksCount(
                    userId,
                    token);

                break;
            case ReadingListStatus.ToRead:
                await profileService.IncrementToReadBooksCount(
                    userId,
                    token);
                break;
            case ReadingListStatus.CurrentlyReading:
                await profileService.IncrementCurrentlyReadingBooksCount(
                    userId,
                    token);

                break;
            default:
                break;
        }

        return true;
    }

    public async Task<Result> Delete(
        ReadingListServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var mapEntity = await data
            .ReadingLists
            .FirstOrDefaultAsync(
                rl =>
                    rl.UserId == userId &&
                    rl.BookId == bookId &&
                    rl.Status == readingStatus,
                token);

        if (mapEntity is null) 
        {
            return BookNotInTheList;
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(token);

        switch (readingStatus)
        {
            case ReadingListStatus.Read:
                await profileService.DecrementReadBooksCount(
                    userId,
                    token);

                break;
            case ReadingListStatus.ToRead:
                await profileService.DecrementToReadBooksCount(
                    userId,
                    token);
                break;
            case ReadingListStatus.CurrentlyReading:
                await profileService.DecrementCurrentlyReadingBooksCount(
                    userId,
                    token);

                break;
            default:
                break;
        }

        return true;
    }

    private async Task<bool> BookIsValid(
        Guid id,
        CancellationToken token = default)
        => await data
            .Books
            .AsNoTracking()
            .AnyAsync(b => b.Id == id, token);
}
