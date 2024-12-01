namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using static Common.Constants.Validation.Nationality;

    public class Nationality : Entity<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Author> Authors { get; } = new HashSet<Author>();
    }
}
