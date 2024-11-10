namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BookHub.Server.Data.Models.Base;

    using static BookHub.Server.Common.Validation.Validation.GenreValidation;

    public class Genre : DeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!; 

        public ICollection<Book> Books { get; } = new HashSet<Book>();
    }
}