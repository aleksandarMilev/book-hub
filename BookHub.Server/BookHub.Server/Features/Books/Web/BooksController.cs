#pragma warning disable ASP0023 
namespace BookHub.Server.Features.Books.Web
{
    using AutoMapper;
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
        ICurrentUserService userService,
        IMapper mapper) : ApiController
    {
        private readonly IBookService bookService = bookService;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListServiceModel>>> All()
            => this.Ok(await this.bookService.GetAllAsync());

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BookListServiceModel>>> TopThree()
           => this.Ok(await this.bookService.GetTopThreeAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.bookService.GetDetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            serviceModel.CreatorId = this.userService.GetId()!;

            var bookId = await this.bookService.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            serviceModel.CreatorId = this.userService.GetId()!;

            var result = await this.bookService.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.bookService.DeleteAsync(id, this.userService.GetId()!);

            return this.NoContentOrBadRequest(result);
        }
    }
}
