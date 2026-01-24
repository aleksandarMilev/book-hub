namespace BookHub.Features.Chat.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class AddUserToChatWebModel
{
    [Required]
    public string UserId { get; init; } = default!;

    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string ChatName { get; init; } = default!;
}
