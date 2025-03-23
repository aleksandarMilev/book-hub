namespace BookHub.Features.Book.Web.User
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Common.ApiRoutes;
    using static Common.DefaultValues;

    [Authorize]
    public class BookController(
        IBookService service,
        IMapper mapper) : ApiController
    {
        private readonly IBookService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Top)]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> TopThree()
          => this.Ok(await this.service.TopThree());

        [HttpGet(ApiRoutes.ByGenre + Id)]
        public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByGenre(
            int id,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize) 
            => this.Ok(await this.service.ByGenre(id, page, pageSize));

        [HttpGet(ApiRoutes.ByAuthor + Id)]
        public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByAuthor(
           int id,
           int page = DefaultPageIndex,
           int pageSize = DefaultPageSize) 
           => this.Ok(await this.service.ByAuthor(id, page, pageSize));

        [HttpGet(Id)]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.Details(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var bookId = await this.service.Create(serviceModel);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var result = await this.service.Edit(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.Delete(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
