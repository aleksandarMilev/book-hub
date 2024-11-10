namespace BookHub.Server.Features.Books.Service
{
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Features.Books.Service.Models;
    using BookHub.Server.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;

    using static BookHub.Server.Common.Messages.Error.Book;

    public class BookService(BookHubDbContext data) : IBookService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<BookListServiceModel>> GetAllAsync()
            => await this.data
                .Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .Select(b => new BookListServiceModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    ShortDescription = b.ShortDescription,
                    Rating = b.Rating,
                    Genres = b.Genres
                        .Select(g => g.Name)
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
                    UserId = b.CreatorId
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
    }
}
