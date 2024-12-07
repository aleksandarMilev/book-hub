namespace BookHub.Server.Features.Book.Service
{
    using Models;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;

    public interface IBookService : ITransientService
    {
        Task<PaginatedModel<BookServiceModel>> ByGenreAsync(int genreId, int page, int pageSize);

        Task<IEnumerable<BookServiceModel>> TopThreeAsync();

        Task<BookDetailsServiceModel?> DetailsAsync(int id);

        Task<BookDetailsServiceModel?> AdminDetailsAsync(int id);

        Task<int> CreateAsync(CreateBookServiceModel model);

        Task<Result> EditAsync(int id, CreateBookServiceModel model);

        Task<Result> DeleteAsync(int id);

        Task<Result> ApproveAsync(int id);

        Task<Result> RejectAsync(int id);
    }
}
