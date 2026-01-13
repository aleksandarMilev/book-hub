namespace BookHub.Features.Genres.Service;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IGenreService : ITransientService
{
    Task<IEnumerable<GenreNameServiceModel>> Names(
        CancellationToken cancellationToken = default);

    Task<GenreDetailsServiceModel?> Details(
        Guid id,
        CancellationToken cancellationToken = default);
}
