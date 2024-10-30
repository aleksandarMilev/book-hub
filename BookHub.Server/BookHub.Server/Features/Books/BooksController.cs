namespace BookHub.Server.Features.Books
{
    using BookHub.Server.Features;
    using BookHub.Server.Features.Books.Models;
    using BookHub.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : ApiController
    {
        private readonly IBookService bookService;
        private readonly ICurrentUserService userService;

        public BooksController(IBookService bookService, ICurrentUserService userService)
        {
            this.bookService = bookService;
            this.userService = userService;
        }

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
            var userId = this.userService.GetId();
            var bookId = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, model.Title, userId!);   
            
            return this.Created(nameof(this.Create), bookId);
        }
    }
}
