namespace BookHub.Server.Features.Genre.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class GenreController(IGenreService service) : ApiController
    {
        private readonly IGenreService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Names()
           => Ok(await service.NamesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Details(int id)
          => Ok(await service.DetailsAsync(id));
    }
}
