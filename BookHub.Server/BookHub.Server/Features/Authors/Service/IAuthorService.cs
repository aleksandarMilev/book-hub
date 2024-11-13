namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IAuthorService
    {
        IEnumerable<string> GetNationalities();

        Task<IEnumerable<AuthorServiceModel>> GetTopThreeAsync();

        Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model);

        Task<Result> EditAsync(int id, CreateAuthorServiceModel model);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
