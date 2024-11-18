namespace BookHub.Server.Features.Genre.Service
{

    using Models;

    public interface IGenreService
    {
        Task<IEnumerable<GenreNameServiceModel>> GetNamesAsync();
    }
}
