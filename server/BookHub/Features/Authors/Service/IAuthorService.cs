namespace BookHub.Features.Authors.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IAuthorService : ITransientService
{
    Task<IEnumerable<AuthorNamesServiceModel>> Names(
        CancellationToken token = default);

    Task<IEnumerable<AuthorServiceModel>> TopThree(
        CancellationToken token = default);

    Task<AuthorDetailsServiceModel?> Details(
        Guid id,
        CancellationToken token = default);

    Task<AuthorDetailsServiceModel?> AdminDetails(
        Guid id,
        CancellationToken token = default);

    Task<AuthorDetailsServiceModel> Create(
        CreateAuthorServiceModel serviceModel,
        CancellationToken token = default);

    Task<Result> Edit(
        Guid id,
        CreateAuthorServiceModel serviceModel,
        CancellationToken token = default);

    Task<Result> Delete(
        Guid authorId,
        CancellationToken token = default);

    Task<Result> Approve(
        Guid id,
        CancellationToken token = default);
    Task<Result> Reject(
        Guid id,
        CancellationToken token = default);

}
