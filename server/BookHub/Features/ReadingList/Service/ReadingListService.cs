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
        var readingStatusIsInvalid = !Enum.IsDefined(typeof(ReadingListStatus), status);
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

    public async Task<Result> Add(
        ReadingListServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var bookId = serviceModel.BookId;
        var readingStatus = serviceModel.Status;

        var isCurrentlyReading = serviceModel.Status == ReadingListStatus.CurrentlyReading;
        var userHasMoreThanFiveCurrentlyReading = await profileService
            .HasMoreThanFiveCurrentlyReading(userId, token);

        if (isCurrentlyReading && userHasMoreThanFiveCurrentlyReading)
        {
            return MoreThanFiveCurrentlyReading;
        }
      
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

        await profileService.UpdateCount(
            userId!,
            GetPropertyName(readingStatus),
            x => ++x,
            token);

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

        await profileService.UpdateCount(
           userId!,
           GetPropertyName(readingStatus),
           x => --x,
           token);

        return true;
    }

    private async Task<bool> BookIsValid(
        Guid id,
        CancellationToken token = default)
        => await data
            .Books
            .AnyAsync(b => b.Id == id, token);

    private static string GetPropertyName(ReadingListStatus status)
        => status switch
    {
        ReadingListStatus.Read => nameof(UserProfile.ReadBooksCount),
        ReadingListStatus.ToRead => nameof(UserProfile.ToReadBooksCount),
        ReadingListStatus.CurrentlyReading => nameof(UserProfile.CurrentlyReadingBooksCount),
        _ => throw new ArgumentOutOfRangeException(
            nameof(status),
            status,
            "Unknown ReadingListStatus value"),
    };
}
