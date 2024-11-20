namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;

    using static Common.Constants.Validation.Review;

    public class Review : DeletableEntity
    {
        public int Id { get; set; }

        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        public double Rating { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public User Creator { get; set; } = null!;

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;

        public ICollection<Reply> Replies { get; } = new HashSet<Reply>();
    }
}
