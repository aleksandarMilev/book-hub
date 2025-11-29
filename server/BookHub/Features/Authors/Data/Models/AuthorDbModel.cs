namespace BookHub.Features.Authors.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using Infrastructure.Services.ImageWriter.Models.Image;
using Features.Book.Data.Models;
using Features.Identity.Data.Models;
using Shared;

using static Shared.Constants.Validation;

public class AuthorDbModel:
    DeletableEntity<Guid>,
    IApprovableEntity,
    IImageDdModel
{
    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public string ImagePath { get; set; } = null!;

    [Required]
    [MaxLength(BiographyMaxLength)]
    public string Biography { get; set; } = null!;

    [MaxLength(PenNameMaxLength)]
    public string? PenName { get; set; }

    public int RatingsCount { get; set; }

    public double AverageRating { get; set; }

    public Nationality Nationality { get; set; }

    public Gender Gender { get; set; }

    public DateTime? BornAt { get; set; }

    public DateTime? DiedAt { get; set; }

    [ForeignKey(nameof(User))]
    public string? CreatorId { get; set; }

    public User? Creator { get; set; }

    public bool IsApproved { get; set; }

    public ICollection<BookDbModel> Books { get; set; } = new HashSet<BookDbModel>();
}
