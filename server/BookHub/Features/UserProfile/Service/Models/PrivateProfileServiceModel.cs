namespace BookHub.Features.UserProfile.Service.Models;

public class PrivateProfileServiceModel : IProfileServiceModel
{
    public string Id { get; init; } = default!;

    public string FirstName { get; init; } = default!;

    public string LastName { get; init; } = default!;

    public string ImagePath { get; init; } = default!;

    public bool IsPrivate { get; init; }
}
