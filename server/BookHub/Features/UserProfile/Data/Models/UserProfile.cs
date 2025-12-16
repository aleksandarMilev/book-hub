namespace BookHub.Features.UserProfile.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using Features.Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models.Image;

using static Shared.Constants.Validation;

public class UserProfile:
    IEntity,
    IImageDdModel
{
    [Key]
    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public UserDbModel User { get; set; } = null!;

    [Required]
    [MaxLength(NameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(NameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(UrlMaxLength)]
    public string? SocialMediaUrl { get; set; }

    [MaxLength(BiographyMaxLength)]
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
