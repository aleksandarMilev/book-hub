namespace BookHub.Features.Reviews.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using Books.Data.Models;
using Identity.Data.Models;

using static Shared.Constants.Validation;

public class ReviewDbModel : DeletableEntity<Guid>
{
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; } = null!;

    public int Rating { get; set; }

    [Required]
    [ForeignKey(nameof(Creator))]
    public string CreatorId { get; set; } = null!;

    public UserDbModel Creator { get; set; } = null!;

    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }

    public BookDbModel Book { get; set; } = null!;

    public ICollection<VoteDbModel> Votes { get; set; } = new HashSet<VoteDbModel>();
}
