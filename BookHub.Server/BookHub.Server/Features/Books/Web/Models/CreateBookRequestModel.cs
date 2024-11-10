namespace BookHub.Server.Features.Books.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    using static BookHub.Server.Common.Validation.Validation.BookValidation;

    public class CreateBookRequestModel
    {
        [Required]
        [StringLength(AuthorMaxLength, MinimumLength = AuthorMinLength)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        //[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(ImageUrlMaxLength, MinimumLength = ImageUrlMinLength)]
        public string ImageUrl { get; set; } = null!;

        public string? UserId { get; set; }
    }
}
