namespace BookHub.Server.Features.Books.Service
{
    using Authors.Service.Models;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Messages.Error.Book;

    public class BookService(BookHubDbContext data) : IBookService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<BookListServiceModel>> GetAllAsync()
            => await this.data
                .Books
                .Select(b => new BookListServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.AverageRating,
                    Genres = this.data
                        .BooksGenres
                        .Where(bg => bg.BookId == b.Id)
                        .Select(bg => bg.Genre.Name)
                        .ToList(),
                    AuthorName = b.Author.Name
                })
                .ToListAsync();

        public async Task<BookDetailsServiceModel?> GetDetailsAsync(int id)
            => await this.data
                .Books
                .Select(b => new BookDetailsServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    LongDescription = b.LongDescription,
                    RatingsCount = b.RatingsCount,
                    AverageRating = b.AverageRating,
                    Genres = this.data
                        .BooksGenres
                        .Where(bg => bg.BookId == b.Id)
                        .Select(bg => bg.Genre.Name)
                        .ToList(),
                    AuthorName = b.Author.Name,
                    Author = new AuthorServiceModel()
                    {
                        Id = b.AuthorId,
                        Name = b.Author.Name,
                        ImageUrl = b.Author.ImageUrl,
                        Biography = b.Author.Biography,
                        BooksCount = b.Author.Books.Count()
                    },
                    CreatorId = b.CreatorId
                })
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> CreateAsync(CreateBookServiceModel model, string userId)
        {
            var book = new Book()
            {
                Title = model.Title,
                ImageUrl = model.ImageUrl,
                CreatorId = userId
            };

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return book.Id;
        }

        public async Task<Result> EditAsync(int id, CreateBookServiceModel model, string userId)
        {
            var book = await this.data
                .Books
                .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != userId)
            {
                return UnauthorizedBookEdit;
            }

            book.Title = model.Title;
            book.ImageUrl = model.ImageUrl;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var book = await this.data
              .Books
              .FindAsync(id);

            if (book is null)
            {
                return BookNotFound;
            }

            if (book.CreatorId != userId)
            {
                return UnauthorizedBookDelete;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<BookListServiceModel>> GetTopThreeAsync()
             => await this.data
                .Books
                .Select(b => new BookListServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    AverageRating = b.AverageRating,
                    Genres = this.data
                        .BooksGenres
                        .Where(bg => bg.BookId == b.Id)
                        .Select(bg => bg.Genre.Name)
                        .ToList(),
                    AuthorName = b.Author.Name
                })
                .OrderByDescending(b => b.AverageRating)
                .Take(3)
                .ToListAsync();

    }
}
