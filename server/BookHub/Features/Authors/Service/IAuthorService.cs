namespace BookHub.Features.Authors.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IAuthorService : ITransientService
    {
        Task<IEnumerable<AuthorNamesServiceModel>> Names();

        Task<IEnumerable<AuthorServiceModel>> TopThree();

        Task<AuthorDetailsServiceModel?> Details(int id);

        Task<AuthorDetailsServiceModel?> AdminDetails(int id);

        Task<int> Create(CreateAuthorServiceModel model);

        Task<Result> Edit(int id, CreateAuthorServiceModel model);

        Task<Result> Delete(int id);

        Task<Result> Approve(int id);

        Task<Result> Reject(int id);
    }
}
