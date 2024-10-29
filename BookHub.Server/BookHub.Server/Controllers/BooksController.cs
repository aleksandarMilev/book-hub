namespace BookHub.Server.Controllers
{
    using BookHub.Server.Controllers.Base;
    using BookHub.Server.Data;
    using BookHub.Server.Data.Models;
    using BookHub.Server.Infrastructure;
    using BookHub.Server.Models.Book;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : ApiController
    {
        private readonly BookHubDbContext data;

        public BooksController(BookHubDbContext data) => this.data = data;

        [HttpPost]
        public async Task<ActionResult<int>> Create(BookFormModel model) 
        {
            var book = new Book()
            {
                UserId = this.User.GetId(),
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Author = model.Author
            };

            this.data.Add(book);
            await this.data.SaveChangesAsync();

            return this.Created(nameof(this.Create), book.Id);
        }
    }
}
