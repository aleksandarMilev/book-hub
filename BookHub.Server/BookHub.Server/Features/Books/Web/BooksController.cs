namespace BookHub.Server.Features.Books.Web
{
    using Infrastructure.Extensions;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class BooksController(
        IBookService bookService,
        ICurrentUserService userService) : ApiController
    {
        private readonly IBookService bookService = bookService;
        private readonly ICurrentUserService userService = userService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListServiceModel>>> All()
        {
            var models = await this.bookService.GetAllAsync();

            return this.Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var model = await this.bookService.GetDetailsAsync(id);

            return this.Ok(model);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult> TopThree()
        {
            var model = await this.bookService.GetTopThreeAsync();

            return this.Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBookWebModel webModel)
        {
            var userId = this.userService.GetId();
            var serviceModel = new CreateBookServiceModel()
            {
                Title = webModel.Title,
                ImageUrl = webModel.ImageUrl,
            };

            var bookId = await this.bookService.CreateAsync(serviceModel, userId!);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var userId = this.userService.GetId();
            var serviceModel = new CreateBookServiceModel()
            {
                Title = webModel.Title,
                ImageUrl = webModel.ImageUrl,
            };

            var result = await this.bookService.EditAsync(id, serviceModel, userId!);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.userService.GetId();
            var result = await this.bookService.DeleteAsync(id, userId!);

            return this.NoContentOrBadRequest(result);
        }
    }
}
