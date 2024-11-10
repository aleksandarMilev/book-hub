namespace BookHub.Server.Features.Books.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static BookHub.Server.Common.Constants.Validation.Book;

    public class CreateBookRequestModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ShortDescriptionMaxLength, MinimumLength = ShortDescriptionMinLength)]
        public string ShortDescription { get; set; } = null!;

        [Required]
        [StringLength(LongDescriptionMaxLength, MinimumLength = LongDescriptionMinLength)]
        public string LongDescription { get; set; } = null!;

        [Range(RatingMinValue, RatingMaxValue)]
        public double Rating { get; set; }

        [Required]
        [StringLength(ImageUrlMaxLength, MinimumLength = ImageUrlMinLength)]
        public string ImageUrl { get; set; } = null!;

        public int AuthorId { get; set; }

        public string? CreatorId { get; set; }
    }
}
