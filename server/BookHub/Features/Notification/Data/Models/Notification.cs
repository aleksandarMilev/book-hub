namespace BookHub.Features.Notification.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using BookHub.Data.Models.Base;
    using Identity.Data.Models;

    using static Shared.ValidationConstants;

    public class Notification : DeletableEntity<int>
    {
        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }

        [Required]
        [ForeignKey(nameof(ReceiverId))]
        public string ReceiverId { get; set; } = null!;

        public User User { get; set; } = null!;

        [ForeignKey(nameof(ResourceType))]
        public Guid ResourceId { get; init; }

        public string ResourceType { get; set; } = null!;
    }
}
