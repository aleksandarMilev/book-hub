namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IAuthorService
    {
        Task<IEnumerable<AuthorNamesServiceModel>> NamesAsync();

        Task<IEnumerable<AuthorServiceModel>> TopThreeAsync();

        Task<AuthorDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model);

        Task<Result> EditAsync(int id, CreateAuthorServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
