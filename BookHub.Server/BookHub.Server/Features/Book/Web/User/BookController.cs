namespace BookHub.Server.Features.Book.Web.User
{
    using AutoMapper;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service;
    using Service.Models;

    [Authorize]
    public class BookController(
        IBookService service,
        IMapper mapper) : ApiController
    {
        private readonly IBookService service = service;
        private readonly IMapper mapper = mapper;

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> TopThree()
          => Ok(await service.TopThreeAsync());

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookServiceModel>>> All()
            => Ok(await service.AllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsServiceModel>> Details(int id)
            => Ok(await service.DetailsAsync(id));

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookWebModel webModel)
        {
            var serviceModel = mapper.Map<CreateBookServiceModel>(webModel);
            var bookId = await service.CreateAsync(serviceModel);

            return Created(nameof(this.Create), bookId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, CreateBookWebModel webModel)
        {
            var serviceModel = mapper.Map<CreateBookServiceModel>(webModel);
            var result = await service.EditAsync(id, serviceModel);

            return this.NoContentOrBadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await service.DeleteAsync(id);

            return this.NoContentOrBadRequest(result);
        }
    }
}
