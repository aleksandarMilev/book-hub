namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using static Common.Constants.Validation.Nationality;

    public class Nationality : Entity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Author> Authors { get; } = new HashSet<Author>();
    }
}
