namespace BookHub.Features.Chat.Service.Models;

using Infrastructure.Services.ImageWriter.Models;

public class CreateChatServiceModel : IImageServiceModel
{
    public string Name { get; init; } = default!;

    public IFormFile? Image { get; init; }
}
