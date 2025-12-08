namespace BookHub.Features.Search.Service.Models;

public class SearchGenreServiceModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string ImagePath { get; init; } = null!;
}
