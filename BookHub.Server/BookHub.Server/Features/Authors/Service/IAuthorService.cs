namespace BookHub.Server.Features.Authors.Service
{
    using Models;

    public interface IAuthorService
    {
        List<string> GetNationalities();

        Task<ICollection<AuthorServiceModel>> GetTopThreeAsync();

        Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model);
    }
}
