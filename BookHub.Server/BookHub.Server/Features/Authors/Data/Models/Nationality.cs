namespace BookHub.Server.Features.Authors.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Server.Data.Models.Base;

    using static Shared.ValidationConstants.Nationality;

    public class Nationality : Entity<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Author> Authors { get; } = new HashSet<Author>();
    }
}
