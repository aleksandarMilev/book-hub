namespace BookHub.Server.Features.Books.Service
{
    using Models;

    public interface IGenreService
    {
        Task<IEnumerable<GenreNameServiceModel>> NamesAsync();
    }
}
