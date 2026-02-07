namespace BookHub.Features.Authors.Service.Models;

using Books.Service.Models;
using Shared;

public class AuthorDetailsServiceModel
{
    public Guid Id { get; init; }

    public int BooksCount { get; init; }

    public double AverageRating { get; init; }

    public string Name { get; init; } = default!;

    public string ImagePath { get; init; } = default!;

    public string Biography { get; init; } = default!;

    public string? PenName { get; init; }

    public Nationality Nationality { get; init; }

    public Gender Gender { get; init; }

    public string? BornAt { get; init; }

    public string? DiedAt { get; init; }

    public string? CreatorId { get; set; }

    public bool IsApproved { get; init; }

    public ICollection<BookServiceModel> TopBooks { get; init; } = new HashSet<BookServiceModel>();
}
