namespace BookHub.Features.UserProfile.Service.Models;

public interface IProfileServiceModel
{
    string Id { get; }

    string FirstName { get; }

    string LastName { get; }

    string ImagePath { get; } 

    bool IsPrivate { get;  }
}
