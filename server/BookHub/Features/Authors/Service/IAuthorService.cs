namespace BookHub.Features.Authors.Service;

using Infrastructure.Services.Result;
using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IAuthorService : ITransientService
{
    Task<IEnumerable<AuthorNamesServiceModel>> Names(
        CancellationToken cancelationToken = default);

    Task<IEnumerable<AuthorServiceModel>> TopThree(
        CancellationToken cancelationToken = default);

    Task<AuthorDetailsServiceModel?> Details(
        Guid authorId,
        CancellationToken cancelationToken = default);

    Task<AuthorDetailsServiceModel?> AdminDetails(
        Guid authorId,
        CancellationToken cancelationToken = default);

    Task<ResultWith<AuthorDetailsServiceModel>> Create(
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancelationToken = default);

    Task<Result> Edit(
        Guid authorId,
        CreateAuthorServiceModel serviceModel,
        CancellationToken cancelationToken = default);

    Task<Result> Delete(
        Guid authorId,
        CancellationToken cancelationToken = default);

    Task<Result> Approve(
        Guid authorId,
        CancellationToken cancelationToken = default);

    Task<Result> Reject(
        Guid authorId,
        CancellationToken cancelationToken = default);
}
