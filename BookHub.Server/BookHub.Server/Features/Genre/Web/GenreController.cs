﻿namespace BookHub.Server.Features.Genre.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service;
    using Service.Models;

    [Authorize]
    public class GenreController(IGenreService service) : ApiController
    {
        private readonly IGenreService service = service;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Names()
           => this.Ok(await this.service.NamesAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GenreNameServiceModel>>> Details(int id)
          => this.Ok(await this.service.DetailsAsync(id));
    }
}
