namespace BookHub.Features.Genres.Service.Models;

using Books.Service.Models;

public class GenreDetailsServiceModel : GenreNameServiceModel
{
    public string Description { get; init; } = default!;

    public string ImagePath { get; init; } = default!;

    public IEnumerable<BookServiceModel> TopBooks { get; init; } = new HashSet<BookServiceModel>();
}
