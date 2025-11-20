namespace BookHub.Features.Book.Service
{
    using Areas.Admin.Service;
    using AutoMapper;
    using BookHub.Data;
    using BookHub.Data.Models.Shared.BookGenre;
    using Data.Models;
    using Features.UserProfile.Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.Services.CurrentUser;
    using Infrastructure.Services.Result;
    using Mapper;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Notification.Service;
    using UserProfile.Service;

    using static Common.Constants.ErrorMessages;

    public class BookService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IAdminService adminService,
        INotificationService notificationService,
        IProfileService profileService,
        IMapper mapper) : IBookService
    {
        private const string DefaultImageUrl = "https://images.alphacoders.com/115/1159192.jpg";
        private const int TopThreeBooksCount = 3;
        private const string OtherGenreName = "Other";

        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private readonly INotificationService notificationService = notificationService;
        private readonly IProfileService profileService = profileService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<BookServiceModel>> TopThree()
            => await this.data
                .Books
                .ToServiceModel()
                .OrderByDescending(b => b.AverageRating)
                .Take(TopThreeBooksCount)
                .ToListAsync();

        public async Task<PaginatedModel<BookServiceModel>> ByGenre(
            int genreId,
            int page,
            int pageSize)
        {
            var books = this.data
                .Books
                .Where(b => b.BooksGenres.Any(bg => bg.GenreId == genreId))
                .ToServiceModel();

            books = books.OrderByDescending(b => b.AverageRating);

            var totalBooks = await books.CountAsync();

            var paginatedBooks = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<BookServiceModel>(
                paginatedBooks,
                totalBooks,
                page,
                pageSize);
        }

        public async Task<PaginatedModel<BookServiceModel>> ByAuthor(
            int authorId, 
            int page, 
            int pageSize)
        {
            var books = this.data
                .Books
                .Where(b => b.AuthorId == authorId)
                .ToServiceModel();

            books = books.OrderByDescending(b => b.AverageRating);

            var totalBooks = await books.CountAsync();

            var paginatedBooks = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<BookServiceModel>(
                paginatedBooks, 
                totalBooks, 
                page, 
                pageSize);
        }

        public async Task<BookDetailsServiceModel?> Details(int id)
            => await this.data
                .Books
                .AsQueryable()
                .MapToDetailsModel(this.userService.GetId()!)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<BookDetailsServiceModel?> AdminDetails(int id)
            => await this.data
                .Books
                .IgnoreQueryFilters()
                .ApplyIsDeletedFilter()
                .MapToDetailsModel(this.userService.GetId()!)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> Create(CreateBookServiceModel model)
        {
            model.ImageUrl ??= DefaultImageUrl;

            var book = this.mapper.Map<Book>(model);
            book.CreatorId = this.userService.GetId();
            book.AuthorId = await this.MapAuthorToBook(model.AuthorId);

            var userIsAdmin = this.userService.IsAdmin();

            if (userIsAdmin)
            {
                book.IsApproved = true;
            }

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            await this.MapBookAndGenres(book.Id, model.Genres);

            if (!userIsAdmin)
            {
                await this.notificationService.CreateOnEntityCreation(
                    book.Id,
                    nameof(Book),
                    book.Title,
                    await this.adminService.GetId());
            }

            return book.Id;
        }

        public async Task<Result> Edit(int id, CreateBookServiceModel model)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Book),
                    id);
            }

            var currentUserIsNotCreator = book.CreatorId != this.userService.GetId()!;

            if (currentUserIsNotCreator)
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Book),
                    id);
            }

            model.ImageUrl ??= DefaultImageUrl;
            this.mapper.Map(model, book);
            book.AuthorId = await this.MapAuthorToBook(model.AuthorId);

            await this.data.SaveChangesAsync();

            await this.MapBookAndGenres(book.Id, model.Genres);

            return true;
        }

        public async Task<Result> Delete(int id)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Book),
                    id);
            }

            var isNotCreator = book.CreatorId != this.userService.GetId();
            var isNotAdmin = !this.userService.IsAdmin();

            if (isNotCreator && isNotAdmin)
            {
                return string.Format(
                    UnauthorizedDbEntityAction,
                    this.userService.GetUsername(),
                    nameof(Book),
                    id);
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> Approve(int id)
        {
            var book = await this.data
                 .Books
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (book is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Book),
                    id);
            }

            book.IsApproved = true;
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChange(
                id,
                nameof(Book),
                book.Title,
                book.CreatorId!,
                true);

            await this.profileService.UpdateCount(
                book.CreatorId!,
                nameof(UserProfile.CreatedBooksCount),
                x => ++x);

            return true;
        }

        public async Task<Result> Reject(int id)
        {
            var book = await this.data
                 .Books
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (book is null)
            {
                return string.Format(
                    DbEntityNotFound,
                    nameof(Book),
                    id);
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChange(
                id,
                nameof(Book),
                book.Title,
                book.CreatorId!,
                false);

            return true;
        }

        private async Task<int?> MapAuthorToBook(int? id) 
        {
            var authorId = await this.data
                .Authors
                .Select(a => a.Id)
                .FirstOrDefaultAsync(a => a == id);

            if (authorId == 0)
            {
                return null;
            }

            return authorId;
        }

        private async Task MapBookAndGenres(
            int bookId, 
            IEnumerable<int> genreIds)
        {
            await this.RemoveExistingBookGenres(bookId);

            if (genreIds.Any())
            {
                foreach (var genreId in genreIds)
                {
                    if (await this.BookGenreExists(bookId, genreId))
                    {
                        continue;
                    }

                    var bookGenre = new BookGenre()
                    {
                        BookId = bookId,
                        GenreId = genreId
                    };

                    this.data.Add(bookGenre);
                }
            }
            else
            {
                var otherGenreId = await this.data
                    .Genres
                    .Where(g => g.Name == OtherGenreName)
                    .Select(g => g.Id)
                    .FirstOrDefaultAsync();

                if (!await this.BookGenreExists(bookId, otherGenreId))
                {
                    var bookGenre = new BookGenre()
                    {
                        BookId = bookId,
                        GenreId = otherGenreId
                    };

                    this.data.Add(bookGenre);
                }
            }

            await this.data.SaveChangesAsync();
        }

        private async Task<bool> BookGenreExists(int bookId, int genreId)
            => await this.data
                .BooksGenres
                .AsNoTracking()
                .AnyAsync(bg =>
                bg.BookId == bookId &&
                bg.GenreId == genreId);

        private async Task RemoveExistingBookGenres(int bookId) 
        {
            var existingMaps = await this.data
               .BooksGenres
               .Where(bg => bg.BookId == bookId)
               .ToListAsync();

            this.data.RemoveRange(existingMaps);
            await this.data.SaveChangesAsync();
        }
    }
}
