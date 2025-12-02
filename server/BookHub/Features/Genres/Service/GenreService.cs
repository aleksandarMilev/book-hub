namespace BookHub.Features.Genre.Service;

using BookHub.Data;
using Shared;
using Microsoft.EntityFrameworkCore;
using Models;

public class GenreService(BookHubDbContext data) : IGenreService
{
    public async Task<IEnumerable<GenreNameServiceModel>> Names(
        CancellationToken token = default)
      => await data
          .Genres
          .ToNameServiceModels()
          .ToListAsync(token);

    public async Task<GenreDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default)
       => await data
           .Genres
           .ToDetailsServiceModels()
           .FirstOrDefaultAsync(g => g.Id == id, token);
}
