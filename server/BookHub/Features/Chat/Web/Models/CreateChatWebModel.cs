namespace BookHub.Features.Chat.Web.Models;

using System.ComponentModel.DataAnnotations;

using static Shared.Constants.Validation;

public class CreateChatWebModel
{
    [Required]
    [StringLength(
        NameMaxLength,
        MinimumLength = NameMinLength)]
    public string Name { get; init; } = default!;

    public IFormFile? Image { get; init; }
}
