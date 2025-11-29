namespace BookHub.Features.Book.Service
{
    using Models;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using BookHub.Infrastructure.Services.Result;

    public interface IBookService : ITransientService
    {
        Task<IEnumerable<BookServiceModel>> TopThree();

        Task<BookDetailsServiceModel?> Details(Guid id);

        Task<BookDetailsServiceModel?> AdminDetails(Guid id);

        Task<PaginatedModel<BookServiceModel>> ByGenre(
            int genreId, 
            int page, 
            int pageSize);

        Task<PaginatedModel<BookServiceModel>> ByAuthor(
            Guid authorId, 
            int page, 
            int pageSize);

        Task<Guid> Create(CreateBookServiceModel model);

        Task<Result> Edit(Guid id, CreateBookServiceModel model);

        Task<Result> Delete(Guid id);

        Task<Result> Approve(Guid id);

        Task<Result> Reject(Guid id);
    }
}
