namespace BookHub.Server.Features.Authors.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IAuthorService : ITransientService
    {
        Task<IEnumerable<AuthorNamesServiceModel>> NamesAsync();

        Task<IEnumerable<AuthorServiceModel>> TopThreeAsync();

        Task<AuthorDetailsServiceModel?> DetailsAsync(int id);

        Task<AuthorDetailsServiceModel?> AdminDetailsAsync(int id);

        Task<int> CreateAsync(CreateAuthorServiceModel model);

        Task<Result> EditAsync(int id, CreateAuthorServiceModel model);

        Task<Result> DeleteAsync(int id);

        Task<Result> ApproveAsync(int id);

        Task<Result> RejectAsync(int id);
    }
}
