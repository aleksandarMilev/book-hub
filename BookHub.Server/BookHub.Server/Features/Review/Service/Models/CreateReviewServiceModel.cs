namespace BookHub.Server.Features.Review.Service.Models
{
    public class CreateReviewServiceModel
    {
        public string Content { get; set; } = null!;

        public double Rating { get; init; }

        public int BookId { get; init; }

        public string CreatorId { get; set; } = null!;
    }
}
