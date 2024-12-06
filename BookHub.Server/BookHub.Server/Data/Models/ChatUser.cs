namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(UserId), nameof(ChatId))]
    public class ChatUser
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Chat))]
        public int ChatId { get; set; } 

        public Chat Chat { get; set; } = null!;

        public bool HasAccepted { get; set; }
    }
}
