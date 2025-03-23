namespace BookHub.Features.Genre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using BookHub.Data.Models.Base;
    using BookHub.Data.Models.Shared.BookGenre;

    using static Shared.ValidationConstants;

    public class Genre : DeletableEntity<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public ICollection<BookGenre> BooksGenres { get; } = new HashSet<BookGenre>();
    }
}