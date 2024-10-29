namespace BookHub.Server.Features.Books
{
    using System.ComponentModel.DataAnnotations;

    using static BookHub.Server.Data.Common.Validation.Book;

    public class BookFormModel
    {
        [Required]
        [MaxLength(AuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
    }
}
