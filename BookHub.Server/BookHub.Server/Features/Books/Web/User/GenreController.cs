namespace BookHub.Server.Features.Books.Web.User
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
    }
}
