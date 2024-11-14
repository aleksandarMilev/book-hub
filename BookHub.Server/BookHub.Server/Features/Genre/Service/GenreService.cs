namespace BookHub.Server.Features.Genre.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class GenreService(BookHubDbContext data) : IGenreService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<string>> GetNamesAsync()
          => await this.data
              .Genres
              .Select(g => g.Name)
              .ToListAsync();
    }
}
