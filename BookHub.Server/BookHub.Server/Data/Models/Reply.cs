namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BookHub.Server.Data.Models.Base;
    using Microsoft.EntityFrameworkCore;

    using static BookHub.Server.Common.Validation.Constants.ReplyValidation;

    public class Reply : DeletableEntity
    {
        public int Id { get; set; }

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
