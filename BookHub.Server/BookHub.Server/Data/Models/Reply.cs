namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;
    using Microsoft.EntityFrameworkCore;

    using static Common.Constants.Validation.Reply;

    public class Reply : DeletableEntity<int>
    {
        [MaxLength(ContentMaxLength)]
        public string Context { get; set; } = null!;

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        public User Creator { get; set; } = null!;

        [ForeignKey(nameof(Review))]
        public int ReviewId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Review Review { get; set; } = null!;
    }
}
