namespace BookHub.Features.Genre.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    using static Common.ApiRoutes;

    [Authorize]
    public class GenreController(IGenreService service) : ApiController
    {
        private readonly IGenreService service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Names()
           => this.Ok(await this.service.Names());

        [HttpGet(Id)]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Details(int id)
           => this.Ok(await this.service.Details(id));
    }
}
