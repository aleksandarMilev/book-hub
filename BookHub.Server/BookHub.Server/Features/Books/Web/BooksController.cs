namespace BookHub.Server.Features.Books.Web
{
    using BookHub.Server.Features;
    using BookHub.Server.Features.Books.Service;
    using BookHub.Server.Features.Books.Web.Models;
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
        public async Task<ActionResult> All()
        {
            Thread.Sleep(2_000);
            var model = await this.bookService.GetAllAsync();
            return this.Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            Thread.Sleep(2_000);
            var model = await this.bookService.GetDetailsAsync(id);
            return this.Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBookRequestModel model)
        {
            var bookId = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, model.Title);
            return this.Created(nameof(this.Create), bookId);
        }
    }
}
