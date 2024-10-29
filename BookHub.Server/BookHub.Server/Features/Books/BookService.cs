namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class BookService : IBookService
    {
        private readonly BookHubDbContext data;

        public BookService(BookHubDbContext data) => this.data = data;

        public async Task<IEnumerable<BookListResponseModel>> GetAllAsync()
        {
            return await this.data
                .Books
                .Select(b => new BookListResponseModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    Author = b.Author,
                })
                .ToListAsync();
        }

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
