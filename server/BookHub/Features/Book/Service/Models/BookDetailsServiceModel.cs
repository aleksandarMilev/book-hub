namespace BookHub.Features.Book.Service.Models;

using Authors.Service.Models;
using Review.Service.Models;

public class BookDetailsServiceModel : BookServiceModel
{
    public string? PublishedDate { get; init; }

    public int RatingsCount { get; init; }

    public string LongDescription { get; init; } = null!;

    public string? CreatorId { get; init; }

    public bool MoreThanFiveReviews { get; init; }

    public bool IsApproved { get; init; }

    public string? ReadingStatus { get; init; }

    public AuthorServiceModel? Author { get; init; }

    public ICollection<ReviewServiceModel> Reviews { get; init; } = new HashSet<ReviewServiceModel>();
}
