namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BookHub.Server.Data.Models.Base;

    using static BookHub.Server.Common.Validation.Validation.ReviewValidation;

    public class Review : DeletableEntity
    {
        public int Id { get; set; }

        [MaxLength(ContentMaxLength)]
        public string Context { get; set; } = null!;

        public double Rating { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public User Creator { get; set; } = null!;

        public ICollection<Reply> Replies { get; } = new HashSet<Reply>();
    }
}
