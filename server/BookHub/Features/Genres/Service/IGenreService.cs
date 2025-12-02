namespace BookHub.Features.Genre.Service;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IGenreService : ITransientService
{
    Task<IEnumerable<GenreNameServiceModel>> Names(
        CancellationToken token = default);

    Task<GenreDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default);
}
