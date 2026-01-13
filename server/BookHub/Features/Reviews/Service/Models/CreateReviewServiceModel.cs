namespace BookHub.Features.Reviews.Service.Models;

public class CreateReviewServiceModel
{
    public string Content { get; set; } = null!;

    public int Rating { get; init; }

    public Guid BookId { get; init; }
}
