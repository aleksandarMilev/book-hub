namespace BookHub.Server.Data.Models.Shared.BookGenre
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Features.Book.Data.Models;
    using Features.Genre.Data.Models;

    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(BookId), nameof(GenreId))]
    public class BookGenre
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        public Genre Genre { get; set; } = null!;
    }
}
