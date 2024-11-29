namespace BookHub.Server.Features.Book.Service
{
    using Models;

    public interface IGenreService
    {
        Task<IEnumerable<GenreNameServiceModel>> NamesAsync();
    }
}
