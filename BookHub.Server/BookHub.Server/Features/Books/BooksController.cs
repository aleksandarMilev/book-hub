namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Features;
    using BookHub.Server.Features.Books.Models;
    using BookHub.Server.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : ApiController
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService) => this.bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListServiceModel>>> All()
        {
            var model = await this.bookService.GetAllAsync();
            return this.Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
        {
            var model = await this.bookService.GetDetailsAsync(id);
            return this.Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookRequestModel model)
        {
            var id = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, model.Title, this.User.GetId());        
            return this.Created(nameof(this.Create), id);
        }
    }
}
