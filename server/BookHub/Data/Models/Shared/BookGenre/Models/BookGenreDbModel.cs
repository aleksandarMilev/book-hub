namespace BookHub.Data.Models.Shared.BookGenre.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Features.Book.Data.Models;
using Features.Genre.Data.Models;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(BookId), nameof(GenreId))]
public class BookGenreDbModel
{
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }

    public BookDbModel Book { get; set; } = null!;

    [ForeignKey(nameof(Genre))]
    public Guid GenreId { get; set; }

    public GenreDbModel Genre { get; set; } = null!;
}
