namespace BookHub.Features.Review.Service
{
    using BookHub.Common;
    using BookHub.Infrastructure.Services.Result;
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Models;

    public interface IReviewService : ITransientService
    {
        Task<PaginatedModel<ReviewServiceModel>> AllForBook(
            Guid bookId,
            int pageIndex,
            int pageSize);

        Task<int> Create(CreateReviewServiceModel model);

        Task<Result> Edit(int id, CreateReviewServiceModel model);

        Task<Result> Delete(int id);
    }
}
