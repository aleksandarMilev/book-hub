namespace BookHub.Features.Search.Service.Models;

public class SearchAuthorServiceModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string? PenName { get; init; }

    public string ImagePath { get; init; } = null!;

    public double AverageRating { get; init; }
}
