namespace BookHub.Features.Books.Service;

using Areas.Admin.Service;
using BookHub.Data;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Common;
using Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.PageClamper;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Notifications.Service;
using Shared;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Common.Utils;
using static Shared.BookMapping;
using static Shared.Constants.Paths;

public class BookService(
    BookHubDbContext data,
    IImageWriter imageWriter,
    IAdminService adminService,
    ICurrentUserService userService,
    INotificationService notificationService,
    IProfileService profileService,
    IPageClamper pageClamper,
    ILogger<BookService> logger) : IBookService
{
    public async Task<IEnumerable<BookServiceModel>> TopThree(
        CancellationToken cancellationToken)
        => await data
            .Books
            .AsNoTracking()
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating)
            .Take(3)
            .ToListAsync(cancellationToken);

    public async Task<PaginatedModel<BookServiceModel>> ByGenre(
        Guid genreId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var books = data
            .Books
            .AsNoTracking()
            .Where(b => b.BooksGenres
                .Any(bg => bg.GenreId == genreId))
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating);

        var total = await books.CountAsync(cancellationToken);
        var items = await books
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<BookServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<BookServiceModel>> ByAuthor(
        Guid authorId, 
        int pageIndex, 
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var books = data
            .Books
            .AsNoTracking()
            .Where(b => b.AuthorId == authorId)
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating);

        var total = await books.CountAsync(cancellationToken);
        var items = await books
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<BookServiceModel>(
            items, 
            total, 
            pageIndex, 
            pageSize);
    }

    public async Task<BookDetailsServiceModel?> Details(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId() ?? string.Empty;
        var dbModel = await data
            .Books
            .AsNoTracking()
            .ToDetailsServiceModels(userId)
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (dbModel is null)
        {
            return null;
        }

        var isAdmin = userService.IsAdmin();
        var currentUserId = userService.GetId();
        var isCreator = string.Equals(
            dbModel.CreatorId,
            currentUserId,
            StringComparison.OrdinalIgnoreCase);

        if (!isCreator && !isAdmin)
        {
            return dbModel;
        }

        var pendingDbModel = await data
            .BookEdits
            .AsNoTracking()
            .FirstOrDefaultAsync(
                b => b.BookId == bookId,
                cancellationToken);

        if (pendingDbModel is null)
        {
            return dbModel;
        }

        return ApplyPendingEdit(dbModel, pendingDbModel);
    }


    public async Task<BookDetailsServiceModel?> AdminDetails(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var userId = userService.GetId()!;
        var dbModel = await data
            .Books
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .ToDetailsServiceModels(userId)
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (dbModel is null)
        {
            return null;
        }

        var pendingDbModel = await data
            .BookEdits
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                b => b.BookId == bookId,
                cancellationToken);

        return pendingDbModel is null
            ? dbModel
            : ApplyPendingEdit(dbModel, pendingDbModel);
    }

    public async Task<BookDetailsServiceModel> Create(
        CreateBookServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var dbModel = serviceModel.ToDbModel();
        var userId = userService.GetId()!;

        dbModel.CreatorId = userId;
        dbModel.AuthorId = await this.MapAuthorToBook(
            serviceModel.AuthorId,
            cancellationToken);

        var isAdmin = userService.IsAdmin();
        if (isAdmin)
        {
            dbModel.IsApproved = true;
        }

        await imageWriter.Write(
           ImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           cancellationToken);

        data.Add(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        var genredIds = (serviceModel.Genres ?? [])
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        await this.MapBookAndGenres(
            dbModel.Id,
            genredIds,
            cancellationToken);

        logger.LogInformation(
            "New book with Id: {id} was created.",
            dbModel.Id);

        if (!isAdmin)
        {
            await notificationService.CreateOnBookCreation(
                dbModel.Id,
                dbModel.Title,
                await adminService.GetId(),
                cancellationToken);
        }

        return await data
            .Books
            .AsNoTracking()
            .ToDetailsServiceModels(userId)
            .SingleOrDefaultAsync(
                b => b.Id == dbModel.Id,
                cancellationToken) 
            ?? throw new InvalidOperationException($"Created book with Id: {dbModel.Id} could not be loaded.");
    }

    public async Task<Result> Edit(
        Guid id,
        CreateBookServiceModel serviceModel,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(id, cancellationToken);
        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(id, userId);
        }

        var pending = await data
            .BookEdits
            .FirstOrDefaultAsync(
                b => b.BookId == id,
                cancellationToken);

        if (pending is null)
        {
            pending = new()
            {
                BookId = id,
                RequestedById = userId,
                ImagePath = dbModel.ImagePath
            };

            data.BookEdits.Add(pending);
        }
        else
        {
            pending.RequestedById = userId;
        }

        var oldPendingImagePath = pending.ImagePath;
        var newImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdatePendingDbModel(pending);

        pending.AuthorId = await this.MapAuthorToBook(
            serviceModel.AuthorId,
            cancellationToken);

        if (newImageUploaded)
        {
            await imageWriter.Write(
                resourceName: PendingImagePathPrefix,
                dbModel: pending,
                serviceModel,
                defaultImagePath: null,
                cancellationToken);

            var shouldDeleteOldPendingImage =
                !string.Equals(
                    oldPendingImagePath,
                    pending.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldPendingImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldPendingImagePath,
                    dbModel.ImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeleteOldPendingImage)
            {
                imageWriter.Delete(
                    resourceName: PendingImagePathPrefix,
                    imagePath: oldPendingImagePath,
                    DefaultImagePath);
            }
        }
        else
        {
            pending.ImagePath = dbModel.ImagePath;
        }

        await data.SaveChangesAsync(cancellationToken);

        if (!userService.IsAdmin())
        {
            await notificationService.CreateOnBookEdition(
                dbModel.Id,
                dbModel.Title,
                receiverId: await adminService.GetId(),
                cancellationToken);
        }

        logger.LogInformation(
            "Book edit request created/updated for BookId: {id}. Pending edit id: {pendingId}",
            dbModel.Id,
            pending.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await this.GetDbModel(
            bookId,
            cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(bookId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(bookId, userId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Approve(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(bookId);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(
                bookId,
                userService.GetId()!);
        }

        var pending = await data
            .BookEdits
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                b => b.BookId == bookId,
                cancellationToken);

        if (pending is not null)
        {
            var oldBookImagePath = dbModel.ImagePath;

            pending.ApplyPendingToBook(dbModel);

            dbModel.AuthorId = await this.MapAuthorToBook(
                dbModel.AuthorId,
                cancellationToken);

            data.BookEdits.Remove(pending);

            dbModel.IsApproved = true;
            await data.SaveChangesAsync(cancellationToken);

            await this.MapBookAndGenres(
                dbModel.Id,
                pending.GetPendingGenres(),
                cancellationToken);

            var shouldDeleteOldBookImage =
                !string.Equals(
                    oldBookImagePath,
                    dbModel.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    oldBookImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeleteOldBookImage)
            {
                imageWriter.Delete(
                    resourceName: ImagePathPrefix,
                    imagePath: oldBookImagePath,
                    DefaultImagePath);
            }

            logger.LogInformation(
                "Pending edit for BookId: {id} approved and applied.",
                dbModel.Id);

            await notificationService.CreateOnBookApproved(
                bookId,
                dbModel.Title,
                receiverId: dbModel.CreatorId!,
                cancellationToken);

            return true;
        }

        var wasPreviouslyUnapproved = !dbModel.IsApproved;

        dbModel.IsApproved = true;
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnBookApproved(
            bookId,
            dbModel.Title,
            receiverId: dbModel.CreatorId!,
            cancellationToken);

        if (wasPreviouslyUnapproved)
        {
            await profileService.IncrementCreatedBooksCount(
                dbModel.CreatorId!,
                cancellationToken);
        }

        return true;
    }

    public async Task<Result> Reject(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(bookId);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(
                bookId,
                userService.GetId()!);
        }

        var pending = await data
            .BookEdits
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(
                a => a.BookId == bookId,
                cancellationToken);

        if (pending is not null)
        {
            var pendingImagePath = pending.ImagePath;

            data.BookEdits.Remove(pending);
            await data.SaveChangesAsync(cancellationToken);

            var shouldDeletePendingImage =
                !string.Equals(
                    pendingImagePath,
                    dbModel.ImagePath,
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    pendingImagePath,
                    DefaultImagePath,
                    StringComparison.OrdinalIgnoreCase);

            if (shouldDeletePendingImage)
            {
                imageWriter.Delete(
                    resourceName: PendingImagePathPrefix,
                    imagePath: pendingImagePath,
                    DefaultImagePath);
            }

            logger.LogInformation(
                "Pending edit for BookId: {id} was rejected (book unchanged).",
                dbModel.Id);

            await notificationService.CreateOnBookRejected(
                bookId,
                dbModel.Title,
                receiverId: dbModel.CreatorId!,
                cancellationToken);

            return true;
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was rejected.",
            dbModel.Id);

        await notificationService.CreateOnBookRejected(
            bookId,
            dbModel.Title,
            receiverId: dbModel.CreatorId!,
            cancellationToken);

        return true;
    }

    private async Task<Guid?> MapAuthorToBook(
        Guid? authorId,
        CancellationToken cancellationToken = default)
    {
        var isNullOrEmptyGuid = 
            authorId is null || 
            authorId == Guid.Empty;

        if (isNullOrEmptyGuid)
        {
            return null;
        }

        var exists = await data
            .Authors
            .AsNoTracking()
            .AnyAsync(
                a => a.Id == authorId,
                cancellationToken);

        return exists ? authorId : null;
    }

    private async Task MapBookAndGenres(
        Guid bookId,
        ICollection<Guid> genreIds,
        CancellationToken cancellationToken = default)
    {
        var strategy = data.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await data
                .Database
                .BeginTransactionAsync(cancellationToken);

            await data.BooksGenres
                .Where(bg => bg.BookId == bookId)
                .ExecuteDeleteAsync(cancellationToken);

            if (genreIds.Count == 0)
            {
                var OtherGenreId = new Guid("52e607d4-c347-440a-8d55-cf2e01d88a6c");
                genreIds.Add(OtherGenreId);
            }

            var mapModels = genreIds
                .Select(genreId => new BookGenreDbModel
                {
                    BookId = bookId,
                    GenreId = genreId
                });

            data.BooksGenres.AddRange(mapModels);

            await data.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static BookDetailsServiceModel ApplyPendingEdit(
        BookDetailsServiceModel baseModel,
        BookEditDbModel pending)
        => new()
        {
            Id = baseModel.Id,
            AverageRating = baseModel.AverageRating,
            RatingsCount = baseModel.RatingsCount,
            Reviews = baseModel.Reviews,
            MoreThanFiveReviews = baseModel.MoreThanFiveReviews,
            ReadingStatus = baseModel.ReadingStatus,
            CreatorId = baseModel.CreatorId,
            IsApproved = baseModel.IsApproved,
            Title = pending.Title,
            ShortDescription = pending.ShortDescription,
            LongDescription = pending.LongDescription,
            Pages = pending.Pages,
            PublishedDate = pending.PublishedDate.ToIso8601String(),
            ImagePath = pending.ImagePath,
            AuthorName = baseModel.AuthorName,
            Genres = baseModel.Genres,
            Author = baseModel.Author
        };

    private string LogAndReturnNotFoundMessage(Guid bookId)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(BookDbModel),
            bookId);

        return string.Format(
            DbEntityNotFound,
            nameof(BookDbModel),
            bookId);
    }

    private string LogAndReturnUnauthorizedMessage(
        Guid bookId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(BookDbModel),
            bookId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(BookDbModel),
            bookId);
    }

    private async Task<BookDbModel?> GetDbModel(
        Guid id,
        CancellationToken cancellationToken = default)
        => await data
            .Books
            .FindAsync([id], cancellationToken);
}
