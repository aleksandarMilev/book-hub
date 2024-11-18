namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IAuthorService
    {
        Task<IEnumerable<AuthorNamesServiceModel>> GetNamesAsync();

        Task<IEnumerable<AuthorServiceModel>> GetTopThreeAsync();

        Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model, string userId);

        Task<Result> EditAsync(int id, CreateAuthorServiceModel model, string userId);

        Task<Result> DeleteAsync(int id, string userId);
    }
}
