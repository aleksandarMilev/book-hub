namespace BookHub.Features.Book.Service;

using Areas.Admin.Service;
using BookHub.Data;
using BookHub.Data.Models.Shared.BookGenre;
using BookHub.Infrastructure.Services.ImageWriter;
using Data.Models;
using Features.Authors.Data.Models;
using Features.UserProfile.Data.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Models;
using Notification.Service;
using UserProfile.Service;

using static Common.Constants.ErrorMessages;
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
        CancellationToken token = default)
        => await data
            .Books
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating)
            .Take(3)
            .ToListAsync(token);

    public async Task<PaginatedModel<BookServiceModel>> ByGenre(
        int genreId,
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var books = data
            .Books
            .Where(b => b.BooksGenres
                .Any(bg => bg.GenreId == genreId))
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating);

        var totalBooks = await books.CountAsync(token);
        var paginatedBooks = await books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<BookServiceModel>(
            paginatedBooks,
            totalBooks,
            page,
            pageSize);
    }

    public async Task<PaginatedModel<BookServiceModel>> ByAuthor(
        Guid authorId, 
        int page, 
        int pageSize,
        CancellationToken token = default)
    {
        var books = data
            .Books
            .Where(b => b.AuthorId == authorId)
            .ToServiceModels()
            .OrderByDescending(b => b.AverageRating);

        var totalBooks = await books.CountAsync(token);
        var paginatedBooks = await books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<BookServiceModel>(
            paginatedBooks, 
            totalBooks, 
            page, 
            pageSize);
    }

    public async Task<BookDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default)
        => await data
            .Books
            .AsQueryable()
            .ToDetailsServiceModels(userService.GetId()!)
            .FirstOrDefaultAsync(b => b.Id == id, token);

    public async Task<BookDetailsServiceModel?> AdminDetails(
        Guid id,
        CancellationToken token = default)
        => await data
            .Books
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .ToDetailsServiceModels(userService.GetId()!)
            .FirstOrDefaultAsync(b => b.Id == id, token);

    public async Task<BookDetailsServiceModel> Create(
        CreateBookServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = serviceModel.ToDbModel();
        var userId = userService.GetId()!;

        dbModel.CreatorId = userId;
        dbModel.AuthorId = await this.MapAuthorToBook(
            serviceModel.AuthorId,
            token);

        var isAdmin = userService.IsAdmin();
        if (isAdmin)
        {
            dbModel.IsApproved = true;
        }

        await imageWriter.Write(
           BooksImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           token);

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        await this.MapBookAndGenres(
            dbModel.Id,
            serviceModel.Genres,
            token);

        logger.LogInformation(
            "New book with Id: {id} was created.",
            dbModel.Id);

        if (!isAdmin)
        {
            await notificationService.CreateOnEntityCreation(
                dbModel.Id,
                nameof(BookDbModel),
                dbModel.Title,
                await adminService.GetId());
        }

        return dbModel.ToDetailsServiceModel(userId);
    }

    public async Task<Result> Edit(
        Guid id,
        CreateBookServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(id, token);
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
            BooksImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            token);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(AuthorDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        dbModel.AuthorId = await this.MapAuthorToBook(
            serviceModel.AuthorId,
            token);

        await data.SaveChangesAsync(token);

        await this.MapBookAndGenres(
            dbModel.Id,
            serviceModel.Genres,
            token);

        logger.LogInformation(
            "Book with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid bookId,
        CancellationToken token = default)
    {
        var dbModel = await this.GetDbModel(bookId, token);
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
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Book with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Approve(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await data
             .Books
             .IgnoreQueryFilters()
             .ApplyIsDeletedFilter()
             .FirstOrDefaultAsync(a => a.Id == id, token);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(id, userService.GetId()!);
        }

        dbModel.IsApproved = true;
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Book with Id: {id} was approved.",
            dbModel.Id);

        await notificationService.CreateOnEntityApprovalStatusChange(
            id,
            "Book",
            dbModel.Title,
            dbModel.CreatorId!,
            true);

        await profileService.UpdateCount(
            dbModel.CreatorId!,
            nameof(UserProfile.CreatedBooksCount),
            x => ++x);

        return true;
    }

    public async Task<Result> Reject(
        Guid id,
        CancellationToken token = default)
    {
        var dbModel = await data
            .Books
            .IgnoreQueryFilters()
            .ApplyIsDeletedFilter()
            .FirstOrDefaultAsync(a => a.Id == id, token);

        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(id);
        }

        if (!userService.IsAdmin())
        {
            return LogAndReturnUnauthorizedMessage(id, userService.GetId()!);
        }

        logger.LogInformation(
            "Book with Id: {id} was rejected.",
            dbModel.Id);

        await notificationService.CreateOnEntityApprovalStatusChange(
            id,
            "Book",
            dbModel.Title,
            dbModel.CreatorId!,
            false);

        return true;
    }

    private async Task<Guid?> MapAuthorToBook(
        Guid? id,
        CancellationToken token = default) 
    {
        Guid authorId = await data
            .Authors
            .Select(a => a.Id)
            .FirstOrDefaultAsync(a => a == id, token);

        if (authorId == Guid.Empty)
        {
            return null;
        }

        return authorId;
    }

    private async Task MapBookAndGenres(
        Guid bookId, 
        IEnumerable<int> genreIds,
        CancellationToken token = default)
    {
        await this.RemoveExistingBookGenres(bookId, token);

        if (genreIds.Any())
        {
            foreach (var genreId in genreIds)
            {
                var bookGenreExists = !await this.BookGenreExists(
                    bookId,
                    genreId,
                    token);

                if (bookGenreExists)
                {
                    continue;
                }

                var bookGenre = new BookGenre
                {
                    BookId = bookId,
                    GenreId = genreId
                };

                data.Add(bookGenre);
            }
        }
        else
        {
            var otherGenreId = await data
                .Genres
                .Where(g => g.Name == "Other")
                .Select(g => g.Id)
                .FirstOrDefaultAsync(token);

            var bookGenreExists = !await this.BookGenreExists(
                bookId,
                otherGenreId,
                token);

            if (bookGenreExists)
            {
                var bookGenre = new BookGenre
                {
                    BookId = bookId,
                    GenreId = otherGenreId
                };

                data.Add(bookGenre);
            }
        }

        await data.SaveChangesAsync(token);
    }

    private async Task<bool> BookGenreExists(
        Guid bookId,
        int genreId,
        CancellationToken token = default)
        => await data
            .BooksGenres
            .AsNoTracking()
            .AnyAsync(
                bg => bg.BookId == bookId && bg.GenreId == genreId,
                token);

    private async Task RemoveExistingBookGenres(
        Guid bookId,
        CancellationToken token = default) 
    {
        var existingMaps = await data
           .BooksGenres
           .Where(bg => bg.BookId == bookId)
           .ToListAsync(token);

        data.RemoveRange(existingMaps);
        await data.SaveChangesAsync(token);
    }

    private string LogAndReturnNotFoundMessage(Guid id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(BookDbModel),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(BookDbModel),
            id);
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
        CancellationToken token = default)
        => await data
            .Books
            .FindAsync([id], token);
}
