namespace BookHub.Server.Features.Books.Web
{
    using BookHub.Server.Features;
    using BookHub.Server.Features.Books.Service;
    using BookHub.Server.Features.Books.Web.Models;
    using BookHub.Server.Infrastructure.Extensions;
    using BookHub.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BooksController(IBookService bookService, ICurrentUserService userService) : ApiController
    {
        private readonly IBookService bookService = bookService;
        private readonly ICurrentUserService userService = userService;

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var model = await this.bookService.GetAllAsync();

            return this.Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var model = await this.bookService.GetDetailsAsync(id);

            return this.Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBookRequestModel model)
        {
            var userId = this.userService.GetId();
            var bookId = await this.bookService.CreateAsync(model.Author, model.Description, model.ImageUrl, model.Title, userId!);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateBookRequestModel model)
        {
            var userId = this.userService.GetId();
            var succeed = await this.bookService.EditAsync(id, model.Title, model.Author, model.ImageUrl, model.Description, userId!);

            return this.NoContentOrBadRequest(succeed);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.userService.GetId();
            var succeed = await this.bookService.DeleteAsync(id, userId!);

            return this.NoContentOrBadRequest(succeed);
        }
    }
}
