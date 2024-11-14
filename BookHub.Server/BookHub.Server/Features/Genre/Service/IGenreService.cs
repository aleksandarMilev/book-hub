namespace BookHub.Server.Features.Genre.Service
{
    public interface IGenreService
    {
        Task<IEnumerable<string>> GetNamesAsync();
    }
}
