namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;
    using Microsoft.EntityFrameworkCore;


    using static Common.Constants.Validation.Chat;

    public class Chat : DeletableEntity<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public User Creator { get; set; } = null!;

        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();

        public ICollection<ChatUser> ChatsUsers { get; set; } = new HashSet<ChatUser>();
    }
}
