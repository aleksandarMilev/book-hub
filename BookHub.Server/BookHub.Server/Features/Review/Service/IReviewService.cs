namespace BookHub.Server.Features.Review.Service
{
    using Models;

    public interface IReviewService
    {
        Task<int> CreateAsync(CreateReviewServiceModel model);
    }
}
