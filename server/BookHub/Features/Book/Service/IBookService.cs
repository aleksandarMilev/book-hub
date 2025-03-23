namespace BookHub.Features.Book.Service
{
    using Models;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IBookService : ITransientService
    {
        Task<IEnumerable<BookServiceModel>> TopThree();

        Task<BookDetailsServiceModel?> Details(int id);

        Task<BookDetailsServiceModel?> AdminDetails(int id);

        Task<PaginatedModel<BookServiceModel>> ByGenre(
            int genreId, 
            int page, 
            int pageSize);

        Task<PaginatedModel<BookServiceModel>> ByAuthor(
            int authorId, 
            int page, 
            int pageSize);

        Task<int> Create(CreateBookServiceModel model);

        Task<Result> Edit(int id, CreateBookServiceModel model);

        Task<Result> Delete(int id);

        Task<Result> Approve(int id);

        Task<Result> Reject(int id);
    }
}
