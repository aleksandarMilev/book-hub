namespace BookHub.Server.Features.Books.Web
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class BooksController(
        IBookService service,
        IMapper mapper) : ApiController
    {
        private readonly IBookService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> TopThree()
          => this.Ok(await this.service.TopThreeAsync());

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> All()
            => this.Ok(await this.service.AllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => this.Ok(await this.service.DetailsAsync(id)); 

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var bookId = await this.service.CreateAsync(serviceModel);

            return this.Created(nameof(this.Create), bookId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var serviceModel = this.mapper.Map<CreateBookServiceModel>(webModel);
            var result = await this.service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
