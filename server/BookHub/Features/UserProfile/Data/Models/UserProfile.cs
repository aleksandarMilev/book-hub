namespace BookHub.Features.UserProfile.Data.Models;

using BookHub.Data.Models.Base;
using Features.Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models;

public class UserProfile:
    IDeletableEntity,
    IImageDdModel
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public string UserId { get; set; } = null!;

    public UserDbModel User { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? SocialMediaUrl { get; set; }

    public string? Biography { get; set; }

    public int CreatedBooksCount { get; set; }

    public int CreatedAuthorsCount { get; set; }

    public int ReviewsCount { get; set; }

    public int ReadBooksCount { get; set; }

    public int ToReadBooksCount { get; set; }

    public int CurrentlyReadingBooksCount { get; set; }

    public bool IsPrivate { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
