namespace BookHub.Features.Review.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using BookHub.Data.Models.Base;
    using Features.Identity.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VoteDbModel : Entity<int>
    {
        public bool IsUpvote { get; set; }

        [ForeignKey(nameof(Review))]
        public Guid ReviewId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public ReviewDbModel Review { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public UserDbModel Creator { get; set; } = null!;
    }
}
