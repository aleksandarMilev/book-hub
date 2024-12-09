namespace BookHub.Server.Features.Book.Web.User
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    using static Server.Common.ApiRoutes;
    using static Server.Common.DefaultValues;

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
          => this.Ok(await this.service.TopThreeAsync());

        [HttpGet(ApiRoutes.ByGenre + Id)]
        public async Task<ActionResult<PaginatedModel<BookServiceModel>>> ByGenre(
            int id,
            int page = DefaultPageIndex,
            int pageSize = DefaultPageSize) => this.Ok(await this.service.ByGenreAsync(id, page, pageSize));

        [HttpGet(Id)]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.DetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var bookId = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
