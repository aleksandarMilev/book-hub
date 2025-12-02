namespace BookHub.Features.Genre.Service.Models;

using Book.Service.Models;

public class GenreDetailsServiceModel : GenreNameServiceModel
{
    public string Description { get; init; } = null!;

    public string ImagePath { get; init; } = null!;

    public IEnumerable<BookServiceModel> TopBooks { get; init; } = new HashSet<BookServiceModel>();
}
