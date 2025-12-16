namespace BookHub.Features.Chat.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.ChatUser;
using Features.Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models.Image;
using Microsoft.EntityFrameworkCore;

using static Shared.Constants.Validation;

public class ChatDbModel:
    DeletableEntity<Guid>,
    IImageDdModel
{
    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Creator))]
    public string CreatorId { get; set; } = null!;

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public UserDbModel Creator { get; set; } = null!;

    public ICollection<ChatMessageDbModel> Messages { get; set; } = new HashSet<ChatMessageDbModel>();

    public ICollection<ChatUser> ChatsUsers { get; set; } = new HashSet<ChatUser>();
}
