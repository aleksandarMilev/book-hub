namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Features.Books.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookService : IBookService
    {
        private readonly BookHubDbContext data;

        public BookService(BookHubDbContext data) => this.data = data;

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
              .Where(b => b.Id == id)
              .Select(b => new BookDetailsServiceModel()
              {
                  Id = b.Id,
                  Title = b.Title,
                  ImageUrl = b.ImageUrl,
                  Author = b.Author,
                  Description = b.Description
              })
              .FirstOrDefaultAsync();

        public async Task<int> CreateAsync(string author, string description, string imageUrl, string title, string userId)
        {
            var book = new Book()
            {
                Author = author,
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId,
            };

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return book.Id;
        }
    }
}
