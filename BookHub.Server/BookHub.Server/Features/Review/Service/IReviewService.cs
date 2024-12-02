namespace BookHub.Server.Features.Review.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IReviewService : ITransientService
    {
        Task<PaginatedModel<ReviewServiceModel>> AllForBookAsync(int bookId, int pageIndex, int pageSize);

        Task<int> CreateAsync(CreateReviewServiceModel model);

        Task<Result> EditAsync(int id, CreateReviewServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
