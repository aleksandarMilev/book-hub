namespace BookHub.Features.Notifications.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using Identity.Data.Models;
using Shared;

using static Shared.Constants.Validation;

public class Notification : DeletableEntity<Guid>
{
    [Required]
    [MaxLength(MessageMaxLength)]
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public string ReceiverId { get; set; } = null!;

    public UserDbModel User { get; set; } = null!;

    [ForeignKey(nameof(ResourceType))]
    public Guid ResourceId { get; init; }

    public ResourceType ResourceType { get; set; }
}
