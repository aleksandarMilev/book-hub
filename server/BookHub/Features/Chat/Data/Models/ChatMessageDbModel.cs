namespace BookHub.Features.Chat.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Features.Identity.Data.Models;
using Microsoft.EntityFrameworkCore;
using BookHub.Data.Models.Base;

using static Shared.Constants.Validation;

public class ChatMessageDbModel : DeletableEntity<int>
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
    public Guid ChatId { get; set; }

    public ChatDbModel Chat { get; set; } = null!;
}
