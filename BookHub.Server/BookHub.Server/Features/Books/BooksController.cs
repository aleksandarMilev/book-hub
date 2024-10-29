namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Features;
    using BookHub.Server.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : ApiController
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService) => this.bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListResponseModel>>> All()
        {
            var model = await this.bookService.GetAllAsync();
            return this.Ok(model);
        }                          

        [HttpPost("create")]
        public async Task<ActionResult<int>> Create(BookFormModel model)
        {
            var id = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, model.Title, this.User.GetId());        
            return this.Created(nameof(this.Create), id);
        }
    }
}
