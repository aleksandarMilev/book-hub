namespace BookHub.Features.Book.Service
{
    using Infrastructure.Services.Result;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IBookService : ITransientService
    {
        Task<IEnumerable<BookServiceModel>> TopThree(
            CancellationToken token = default);

        Task<BookDetailsServiceModel?> Details(
            Guid id,
            CancellationToken token = default);

        Task<BookDetailsServiceModel?> AdminDetails(
            Guid id,
            CancellationToken token = default);

        Task<PaginatedModel<BookServiceModel>> ByGenre(
            int genreId, 
            int page, 
            int pageSize,
            CancellationToken token = default);

        Task<PaginatedModel<BookServiceModel>> ByAuthor(
            Guid authorId, 
            int page, 
            int pageSize,
            CancellationToken token = default);

        Task<BookDetailsServiceModel> Create(
            CreateBookServiceModel model,
            CancellationToken token = default);

        Task<Result> Edit(
            Guid id,
            CreateBookServiceModel model,
            CancellationToken token = default);

        Task<Result> Delete(
            Guid id,
            CancellationToken token = default);

        Task<Result> Approve(
            Guid id,
            CancellationToken token = default);

        Task<Result> Reject(
            Guid id,
            CancellationToken token = default);
    }
}
