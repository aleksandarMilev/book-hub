namespace BookHub.Features.Books.Service;

using Areas.Admin.Service;
using BookHub.Common;
using BookHub.Data;
using BookHub.Data.Models.Shared.BookGenre.Models;
using BookHub.Features.Books.Shared;
using Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Notifications.Service;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
using static Common.Utils;
using static Shared.BookMapping;
using static Shared.Constants.Paths;

public class BookService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IAdminService adminService,
    INotificationService notificationService,
    IImageWriter imageWriter,
    ILogger<BookService> logger,
    IProfileService profileService) : IBookService
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
        ClampPageSizeAndIndex(
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
        ClampPageSizeAndIndex(
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
        => await data
            .Books
            .AsNoTracking()
            .AsQueryable()
            .ToDetailsServiceModels(userService.GetId()!)
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

    public async Task<BookDetailsServiceModel?> AdminDetails(
        Guid bookId,
        CancellationToken cancellationToken = default)
        => await data
            .Books
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .ToDetailsServiceModels(userService.GetId()!)
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

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
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
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
        var dbModel = await this.GetDbModel(
            id,
            cancellationToken);

        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(id);
        }

        var userId = userService.GetId()!;
        if (dbModel.CreatorId != userId)
        {
            return LogAndReturnUnauthorizedMessage(id, userId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            ImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            cancellationToken);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(BookDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        dbModel.AuthorId = await this.MapAuthorToBook(
            serviceModel.AuthorId,
            cancellationToken);

        await data.SaveChangesAsync(cancellationToken);

        await this.MapBookAndGenres(
            dbModel.Id,
            serviceModel.Genres,
            cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was updated.",
            dbModel.Id);

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

        dbModel.IsApproved = true;
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnBookApproved(
            bookId,
            dbModel.Title,
            dbModel.CreatorId!,
            cancellationToken);

        await profileService.IncrementCreatedBooksCount(
            dbModel.CreatorId!,
            cancellationToken);

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

        data.Remove(dbModel);
        await data.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Book with Id: {id} was rejected.",
            dbModel.Id);

        await notificationService.CreateOnBookRejected(
            bookId,
            dbModel.Title,
            dbModel.CreatorId!,
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
