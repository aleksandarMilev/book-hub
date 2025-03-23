namespace BookHub.Features.Genre.Service
{
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IGenreService : ITransientService
    {
        Task<IEnumerable<GenreNameServiceModel>> Names();

        Task<GenreDetailsServiceModel?> Details(int id);
    }
}
