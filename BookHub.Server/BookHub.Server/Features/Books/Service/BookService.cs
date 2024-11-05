namespace BookHub.Server.Features.Books.Service
{
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Features.Books.Service.Models;
    using Microsoft.EntityFrameworkCore;

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
                    Author = b.Author,
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
                    Author = b.Author,
                    Description = b.Description,
                    UserId = b.UserId
                })
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<int> CreateAsync(string author, string description, string imageUrl, string title, string userId)
        {
            var book = new Book()
            {
                Author = author,
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId
            };

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return book.Id;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var book = await this.data
              .Books
              .FindAsync(id);

            if (book is null || book.UserId != userId)
            {
                return false;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
