namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IAuthorService
    {
        List<string> GetNationalities();

        Task<ICollection<AuthorServiceModel>> GetTopThreeAsync();

        Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model);

        Task<Result> EditAsync(int id, CreateAuthorServiceModel model);
    }
}
