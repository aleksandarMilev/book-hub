namespace BookHub.Features.Identity.Service.Models;

public class MessageServiceModel(string message)
{
    public string Message { get; init; } = message;
}
