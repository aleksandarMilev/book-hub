namespace BookHub.Features.Review.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BookHub.Data.Models.Base;
    using Features.Book.Data.Models;
    using Features.Identity.Data.Models;

    using static Shared.ValidationConstants;

    public class Review : DeletableEntity<int>
    {
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        public int Rating { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public User Creator { get; set; } = null!;

        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        public BookDbModel Book { get; set; } = null!;

        public ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();
    }
}
