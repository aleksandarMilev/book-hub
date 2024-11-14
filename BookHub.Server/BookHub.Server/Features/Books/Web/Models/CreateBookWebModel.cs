namespace BookHub.Server.Features.Books.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Book;

    public class CreateBookWebModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; init; } = null!;

        [StringLength(AuthorNameMaxLength, MinimumLength = AuthorNameMinLength)]
        public string? AuthorName { get; init; }

        [Required]
        [StringLength(ImageUrlMaxLength, MinimumLength = ImageUrlMinLength)]
        public string ImageUrl { get; init; } = null!;

        [Required]
        [StringLength(ShortDescriptionMaxLength, MinimumLength = ShortDescriptionMinLength)]
        public string ShortDescription { get; init; } = null!;

        [Required]
        [StringLength(LongDescriptionMaxLength, MinimumLength = LongDescriptionMinLength)]
        public string LongDescription { get; init; } = null!;

        public string? PublishedDate { get; init; }
    }
}
