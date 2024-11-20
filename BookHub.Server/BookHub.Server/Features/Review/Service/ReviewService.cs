using AutoMapper;
namespace BookHub.Server.Features.Review.Service
{
    using BookHub.Server.Infrastructure.Services;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ReviewService(
        BookHubDbContext data,
        IMapper mapper) : IReviewService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<int> CreateAsync(CreateReviewServiceModel model)
        {
            await this.ValidateBookId(model.BookId);
            var review = this.mapper.Map<Review>(model);

            this.data.Add(review);
            await this.data.SaveChangesAsync();

            return review.Id;
        }

        private async Task ValidateBookId(int bookId)
        {
            var id = await this.data
               .Books
               .Where(b => b.Id == bookId)
               .Select(b => b.Id)
               .FirstOrDefaultAsync();

            if (id == 0)
            {
                throw new InvalidOperationException("Book not found!");
            }
        }
    }
}
