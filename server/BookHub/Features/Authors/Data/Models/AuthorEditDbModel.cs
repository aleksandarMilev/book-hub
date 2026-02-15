namespace BookHub.Features.Authors.Data.Models;

using BookHub.Data.Models.Base;
using Infrastructure.Services.ImageWriter.Models;
using Shared;

public class AuthorEditDbModel : 
    DeletableEntity<Guid>,
    IImageDdModel
{
    public Guid AuthorId { get; set; }

    public AuthorDbModel Author { get; set; } = default!;

    public string RequestedById { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string Biography { get; set; } = default!;

    public string? PenName { get; set; }

    public Nationality Nationality { get; set; }

    public Gender Gender { get; set; }

    public DateTime? BornAt { get; set; }

    public DateTime? DiedAt { get; set; }
}
