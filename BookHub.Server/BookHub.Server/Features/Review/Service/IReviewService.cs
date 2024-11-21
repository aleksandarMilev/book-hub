namespace BookHub.Server.Features.Review.Service
{
    using Infrastructure.Services;
    using Models;

    public interface IReviewService
    {
        Task<int> CreateAsync(CreateReviewServiceModel model);

        Task<Result> EditAsync(int id, CreateReviewServiceModel model);

        Task<Result> DeleteAsync(int id);
    }
}
