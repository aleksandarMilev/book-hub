namespace BookHub.Features.Chat.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Features.Identity.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using BookHub.Data.Models.Base;

    using static Shared.ValidationConstants;

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
