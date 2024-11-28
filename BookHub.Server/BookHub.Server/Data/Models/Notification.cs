namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;

    using static Common.Constants.Validation.Notification;

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
