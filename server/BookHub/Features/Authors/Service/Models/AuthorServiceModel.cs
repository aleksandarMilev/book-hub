namespace BookHub.Features.Authors.Service.Models;

public class AuthorServiceModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string ImagePath { get; init; } = null!;

    public string Biography { get; init; } = null!;

    public int BooksCount { get; init; }

    public double AverageRating { get; init; }
}
