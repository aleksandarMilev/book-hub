namespace BookHub.Server.Features.Genre.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GenreService(BookHubDbContext data) : IGenreService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<GenreNameServiceModel>> GetNamesAsync()
          => await this.data
              .Genres
              .Select(g => new GenreNameServiceModel() 
              {
                  Id = g.Id,
                  Name = g.Name
              })
              .ToListAsync();
    }
}
