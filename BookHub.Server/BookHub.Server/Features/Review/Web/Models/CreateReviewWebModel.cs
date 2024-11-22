namespace BookHub.Server.Features.Review.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Review;

    public class CreateReviewWebModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; init; } = null!;

        [Range(RatingMinValue, RatingMaxValue)]
        public int Rating { get; init; }

        public int BookId { get; init; }
    }
}
