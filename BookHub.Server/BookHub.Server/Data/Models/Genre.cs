namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using static Common.Constants.Validation.Genre;

    public class Genre : DeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!; 

        public ICollection<Book> Books { get; } = new HashSet<Book>();
    }
}