namespace BookHub.Features.Books.Service
{
    using BookHub.Common;
    using Infrastructure.Services.Result;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IBookService : ITransientService
    {
        Task<IEnumerable<BookServiceModel>> TopThree(
            CancellationToken cancellationToken = default);

        Task<BookDetailsServiceModel?> Details(
            Guid bookId,
            CancellationToken cancellationToken = default);

        Task<BookDetailsServiceModel?> AdminDetails(
            Guid bookId,
            CancellationToken cancellationToken = default);

        Task<PaginatedModel<BookServiceModel>> ByGenre(
            Guid genreId, 
            int page, 
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PaginatedModel<BookServiceModel>> ByAuthor(
            Guid authorId, 
            int pageIndex, 
            int pageSize,
            CancellationToken token = default);

        Task<BookDetailsServiceModel> Create(
            CreateBookServiceModel model,
            CancellationToken cancellationToken = default);

        Task<Result> Edit(
            Guid bookId,
            CreateBookServiceModel model,
            CancellationToken cancellationToken = default);

        Task<Result> Delete(
            Guid bookId,
            CancellationToken cancellationToken = default);

        Task<Result> Approve(
            Guid bookId,
            CancellationToken cancellationToken = default);

        Task<Result> Reject(
            Guid bookId,
            CancellationToken cancellationToken = default);
    }
}
