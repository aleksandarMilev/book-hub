namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;
    using Microsoft.EntityFrameworkCore;

    using static Common.Constants.Validation.Chat;

    public class ChatMessage : DeletableEntity<int>
    {
        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; } = null!;

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public User Sender { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Chat))]
        public int ChatId { get; set; }

        public Chat Chat { get; set; } = null!;
    }
}
