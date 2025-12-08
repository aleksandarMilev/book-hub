namespace BookHub.Features.Review.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateReviewWebModel
{
    [Required]
    [StringLength(
        ContentMaxLength,
        MinimumLength = ContentMinLength)]
    public string Content { get; init; } = null!;

    [Range(RatingMinValue, RatingMaxValue)]
    public int Rating { get; init; }

    public Guid BookId { get; init; }
}
