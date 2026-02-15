namespace BookHub.Features.Books.Service.Models;

using Genres.Service.Models;

public class BookServiceModel
{
    public Guid Id { get; init; }

    public string Title { get; init; } = default!;

    public string? AuthorName { get; init; }

    public string ImagePath { get; init; } = default!;

    public string ShortDescription { get; init; } = default!;

    public double AverageRating { get; init; } 

    public ICollection<GenreNameServiceModel> Genres { get; init; }
        = new HashSet<GenreNameServiceModel>();
}
