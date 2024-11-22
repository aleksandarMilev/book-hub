namespace BookHub.Server.Features.Books.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GenreService(BookHubDbContext data) : IGenreService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<GenreNameServiceModel>> NamesAsync()
          => await data
              .Genres
              .Select(g => new GenreNameServiceModel()
              {
                  Id = g.Id,
                  Name = g.Name
              })
              .ToListAsync();
    }
}
