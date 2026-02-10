namespace BookHub.Features.Genres.Data.Models;

using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.BookGenre.Models;

public class GenreDbModel : DeletableEntity<Guid>
{
    public string Name { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ICollection<BookGenreDbModel> BooksGenres { get; set; } = new HashSet<BookGenreDbModel>();
}