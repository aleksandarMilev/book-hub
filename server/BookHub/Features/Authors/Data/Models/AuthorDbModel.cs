namespace BookHub.Features.Authors.Data.Models;

using BookHub.Data.Models.Base;
using Books.Data.Models;
using Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models;
using Shared;

public class AuthorDbModel:
    DeletableEntity<Guid>,
    IApprovableEntity,
    IImageDdModel
{
    public string Name { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string Biography { get; set; } = default!;

    public string? PenName { get; set; }

    public int RatingsCount { get; set; }

    public double AverageRating { get; set; }

    public Nationality Nationality { get; set; }

    public Gender Gender { get; set; }

    public DateTime? BornAt { get; set; }

    public DateTime? DiedAt { get; set; }

    public string? CreatorId { get; set; }

    public UserDbModel? Creator { get; set; }

    public bool IsApproved { get; set; }

    public ICollection<BookDbModel> Books { get; set; } = new HashSet<BookDbModel>();
}
