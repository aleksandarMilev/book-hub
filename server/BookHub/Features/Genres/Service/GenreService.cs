namespace BookHub.Features.Genres.Service;

using BookHub.Data;
using Shared;
using Microsoft.EntityFrameworkCore;
using Models;

public class GenreService(BookHubDbContext data) : IGenreService
{
    public async Task<IEnumerable<GenreNameServiceModel>> Names(
        CancellationToken cancellationToken = default)
        => await data
            .Genres
            .AsNoTracking()
            .ToNameServiceModels()
            .ToListAsync(cancellationToken);

    public async Task<GenreDetailsServiceModel?> Details(
        Guid genreId,
        CancellationToken cancellationToken = default)
        => await data
            .Genres
            .AsNoTracking()
            .ToDetailsServiceModels()
            .FirstOrDefaultAsync(
                g => g.Id == genreId,
                cancellationToken);
}
