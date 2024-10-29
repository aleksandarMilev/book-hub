namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;

    public class BookService : IBookService
    {
        private readonly BookHubDbContext data;

        public BookService(BookHubDbContext data) => this.data = data;

        public async Task<int> CreateAsync(string author, string description, string imageUrl, string userId)
        {
            var book = new Book()
            {
                Author = author,
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
