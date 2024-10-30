namespace BookHub.Server.Features.Books.Models
{
    using System.ComponentModel.DataAnnotations;

    using static BookHub.Server.Data.Common.Validation.Book;

    public class CreateBookRequestModel
    {
        [Required]
        [MaxLength(AuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; init; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        public string? UserId { get; set; }
    }
}
