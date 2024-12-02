namespace BookHub.Server.Features.Book.Service
{
    using Areas.Admin.Service;
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Mapper;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Notification.Service;
    using UserProfile.Service;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Book;

    public class BookService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IAdminService adminService,
        INotificationService notificationService,
        IProfileService profileService,
        IMapper mapper) : IBookService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IAdminService adminService = adminService;
        private readonly INotificationService notificationService = notificationService;
        private readonly IProfileService profileService = profileService;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<BookServiceModel>> AllAsync()
            => await this.data
                .Books
                .MapToServiceModel()
                .ToListAsync();

        public async Task<IEnumerable<BookServiceModel>> TopThreeAsync()
            => await this.data
                .Books
                .MapToServiceModel()
                .OrderByDescending(b => b.AverageRating)
                .Take(3)
                .ToListAsync();

        public async Task<BookDetailsServiceModel?> DetailsAsync(int id)
            => await this.data
                .Books
                .AsQueryable()
                .MapToDetailsModel(this.userService.GetId()!)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<BookDetailsServiceModel?> AdminDetailsAsync(int id)
            => await this.data
                .Books
                .IgnoreQueryFilters()
                .ApplyIsDeletedFilter()
                .MapToDetailsModel(this.userService.GetId()!)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> CreateAsync(CreateBookServiceModel model)
        {
            model.ImageUrl ??= DefaultBookImageUrl;

            var book = this.mapper.Map<Book>(model);
            book.CreatorId = this.userService.GetId();
            book.AuthorId = await this.MapAuthorToBookAsync(model.AuthorId);

            var userIsAdmin = this.userService.IsAdmin();

            if (userIsAdmin)
            {
                book.IsApproved = true;
            }

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            await this.MapBookAndGenresAsync(book.Id, model.Genres);

            if (!userIsAdmin)
            {
                await this.notificationService.CreateOnEntityCreationAsync(
                    book.Id,
                    nameof(Book),
                    book.Title,
                    await this.adminService.GetIdAsync());
            }

            return book.Id;
        }

        public async Task<Result> EditAsync(int id, CreateBookServiceModel model)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != this.userService.GetId()!)
            {
                return UnauthorizedBookEdit;
            }

            model.ImageUrl ??= DefaultBookImageUrl;
            this.mapper.Map(model, book);
            book.AuthorId = await this.MapAuthorToBookAsync(model.AuthorId);

            await this.data.SaveChangesAsync();

            await this.MapBookAndGenresAsync(book.Id, model.Genres);

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var book = await this.data
                 .Books
                 .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != this.userService.GetId()!)
            {
                return UnauthorizedBookDelete;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> ApproveAsync(int id)
        {
            var book = await this.data
                 .Books
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (book is null)
            {
                return BookNotFound;
            }

            book.IsApproved = true;
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                id,
                nameof(Book),
                book.Title,
                book.CreatorId!,
                true);

            await this.profileService.UpdateCountAsync(
                this.userService.GetId()!,
                nameof(UserProfile.CreatedBooksCount),
                x => ++x);

            return true;
        }

        public async Task<Result> RejectAsync(int id)
        {
            var book = await this.data
                 .Books
                 .IgnoreQueryFilters()
                 .ApplyIsDeletedFilter()
                 .FirstOrDefaultAsync(a => a.Id == id);

            if (book is null)
            {
                return BookNotFound;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            await this.notificationService.CreateOnEntityApprovalStatusChangeAsync(
                id,
                nameof(Book),
                book.Title,
                book.CreatorId!,
                false);

            return true;
        }

        private async Task<int?> MapAuthorToBookAsync(int? id) 
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

        private async Task MapBookAndGenresAsync(int bookId, IEnumerable<int> genreIds)
        {
            await this.RemoveExistingBookGenres(bookId);

            if (genreIds.Any())
            {
                foreach (var genreId in genreIds)
                {
                    if (await this.BookGenreExistsAsync(bookId, genreId))
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

                if (!await this.BookGenreExistsAsync(bookId, otherGenreId))
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

        private async Task<bool> BookGenreExistsAsync(int bookId, int genreId)
            => await this.data
                .BooksGenres
                .AsNoTracking()
                .AnyAsync(bg => bg.BookId == bookId && bg.GenreId == genreId);

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
