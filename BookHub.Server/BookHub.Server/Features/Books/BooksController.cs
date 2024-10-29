namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Features;
    using BookHub.Server.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : ApiController
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService) => this.bookService = bookService;

        [HttpPost]
        public async Task<ActionResult<int>> Create(BookFormModel model)
        {
            var id = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, this.User.GetId());        
            return this.Created(nameof(this.Create), id);
        }
    }
}
