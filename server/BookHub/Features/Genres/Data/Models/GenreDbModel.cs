namespace BookHub.Features.Genre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using BookHub.Data.Models.Base;
    using BookHub.Data.Models.Shared.BookGenre.Models;

    using static Shared.Constants.Validation;

    public class GenreDbModel : DeletableEntity<Guid>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string ImagePath { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public ICollection<BookGenreDbModel> BooksGenres { get; set; } = new HashSet<BookGenreDbModel>();
    }
}