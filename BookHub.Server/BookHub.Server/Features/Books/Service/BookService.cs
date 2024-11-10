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
                    ImageUrl = b.ImageUrl
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
                //Author = model.Author,
                //Title = model.Title,
                //Description = model.Description,
                //ImageUrl = model.ImageUrl,
                //UserId = userId
            };

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return book.Id;
        }

        public async Task<bool> EditAsync(int id, CreateBookServiceModel model, string userId)
        {
            var book = await this.data
                .Books
                .FindAsync(id);

            if (book is null || book.CreatorId != userId)
            {
                return false;
            }

            //book.Title = model.Title;
            //book.Author = model.Author;
            //book.ImageUrl = model.ImageUrl;
            //book.Description = model.Description;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var book = await this.data
              .Books
              .FindAsync(id);

            if (book is null || book.CreatorId != userId)
            {
                return false;
            }

            this.data.Remove(book);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
