namespace BookHub.Server.Features.Notification.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Features.Identity.Data.Models;
    using Server.Data.Models.Base;

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
        public int ResourceId { get; init; }

        public string ResourceType { get; set; } = null!;
    }
}
