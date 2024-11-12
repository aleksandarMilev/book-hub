namespace BookHub.Server.Data.Seed
{
    using Models;

    public class BookGenreSeeder
    {
        public static BookGenre[] Seed()
            => new BookGenre[]
            {
                new() { BookId = 1, GenreId = 1 },
                new() { BookId = 1, GenreId = 6 },
                new() { BookId = 2, GenreId = 3 },
                new() { BookId = 2, GenreId = 14 },
                new() { BookId = 2, GenreId = 15 },
                new() { BookId = 3, GenreId = 3 }
            };
    }
}
