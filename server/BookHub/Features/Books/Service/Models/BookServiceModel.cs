namespace BookHub.Features.Books.Service.Models;

using Genres.Service.Models;

public class BookServiceModel
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string? AuthorName { get; init; }

    public string ImagePath { get; init; } = null!;

    public string ShortDescription { get; init; } = null!;

    public double AverageRating { get; init; } 

    public ICollection<GenreNameServiceModel> Genres { get; init; } = new HashSet<GenreNameServiceModel>();
}
