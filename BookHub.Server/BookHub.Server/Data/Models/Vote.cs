namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;
    using Microsoft.EntityFrameworkCore;

    public class Vote : Entity<int>
    {
        public bool IsUpvote { get; set; }

        [ForeignKey(nameof(Review))]
        public int ReviewId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Review Review { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public User Creator { get; set; } = null!;
    }
}
