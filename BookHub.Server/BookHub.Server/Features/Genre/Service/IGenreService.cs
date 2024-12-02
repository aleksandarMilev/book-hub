namespace BookHub.Server.Features.Genre.Service
{
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IGenreService : ITransientService
    {
        Task<IEnumerable<GenreNameServiceModel>> NamesAsync();

        Task<GenreDetailsServiceModel?> DetailsAsync(int id);
    }
}
