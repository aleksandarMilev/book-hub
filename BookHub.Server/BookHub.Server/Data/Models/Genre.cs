namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using static Common.Constants.Validation.Genre;

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