namespace BookHub.Server.Data.Seed
{
    using Models;

    public class BookGenreSeeder
    {
        public static BookGenre[] Seed()
            => new BookGenre[]
            {
                //pet sematary
                new() { BookId = 1, GenreId = 1 }, //horror
                new() { BookId = 1, GenreId = 4 }, //mystery
                new() { BookId = 1, GenreId = 6 }, //thriller
                new() { BookId = 1, GenreId = 30 }, //supernatural
                //harry potter and the deathly hallows
                new() { BookId = 2, GenreId = 3 }, //fantasy
                new() { BookId = 2, GenreId = 7 }, //adventure
                new() { BookId = 2, GenreId = 14 }, //children
                new() { BookId = 2, GenreId = 15 }, //young adult
                new() { BookId = 2, GenreId = 32 }, //magical realism
                //lord of the rings
                new() { BookId = 3, GenreId = 3 }, //fantasy
                new() { BookId = 3, GenreId = 7 }, //adventure
                new() { BookId = 3, GenreId = 24 }, //epic
                //1984
                new() { BookId = 4, GenreId = 19 }, //dystopian
                new() { BookId = 4, GenreId = 25 }, //Political fiction
                new() { BookId = 4, GenreId = 28 }, //satire
                new() { BookId = 4, GenreId = 29 }, //psychological fiction
                //one flew over a cockoo's nest
                new() { BookId = 5, GenreId = 13 }, //drama
                new() { BookId = 5, GenreId = 19 }, //dystopian
                new() { BookId = 5, GenreId = 26 }, //Philosophical fiction
                new() { BookId = 5, GenreId = 28 }, //satire
                new() { BookId = 5, GenreId = 29 }, //psychological fiction
                //the shining
                new() { BookId = 6, GenreId = 1 }, //horror
                new() { BookId = 6, GenreId = 6 }, //thriller
                new() { BookId = 6, GenreId = 29 }, //psychological fiction
                new() { BookId = 6, GenreId = 30 }, //supernatural
                //dracula
                new() { BookId = 7, GenreId = 1 }, //horror
                new() { BookId = 7, GenreId = 6 }, //thriller
                new() { BookId = 7, GenreId = 7 }, //adventure
                new() { BookId = 7, GenreId = 8 }, //historical
                new() { BookId = 7, GenreId = 30 }, //supernatural
                new() { BookId = 7, GenreId = 31 }, //gothic
                //it
                new() { BookId = 8, GenreId = 1 }, //horror
                new() { BookId = 8, GenreId = 3 }, //fantasy
                new() { BookId = 8, GenreId = 6 }, //thriller
                new() { BookId = 8, GenreId = 7 }, //adventure
                //animal farm
                new() { BookId = 9, GenreId = 19 }, //dystopian
                new() { BookId = 9, GenreId = 25 }, //polytical fiction
                new() { BookId = 9, GenreId = 28 }, //satire
                new() { BookId = 9, GenreId = 29 }, //psychological fiction
                //clockwork orange
                new() { BookId = 10, GenreId = 19 }, //dystopian
                new() { BookId = 10, GenreId = 21 }, //crime
                new() { BookId = 10, GenreId = 26 }, //Philosophical Fiction
                new() { BookId = 10, GenreId = 28 }, //Psychological Fiction
                new() { BookId = 10, GenreId = 29 }, //Satire
                //orient express
                new() { BookId = 11, GenreId = 4 }, //mystery
                new() { BookId = 11, GenreId = 6 }, //thriller
                new() { BookId = 11, GenreId = 21 }, //crime
                //roger acroyd
                new() { BookId = 12, GenreId = 4 }, //mystery
                new() { BookId = 12, GenreId = 6 }, //thriller
                new() { BookId = 12, GenreId = 21 }, //crime
                //east of eden
                new() { BookId = 13, GenreId = 8 }, //Historical
                new() { BookId = 13, GenreId = 13 }, //Drama
                new() { BookId = 13, GenreId = 26 }, //Philosophical
                new() { BookId = 13, GenreId = 28 }, //Psychological
                //dune
                new() { BookId = 14, GenreId = 2 }, //science fiction
                new() { BookId = 14, GenreId = 3 }, //fantasy
                new() { BookId = 14, GenreId = 7 }, //adventure
                new() { BookId = 14, GenreId = 24 }, //epic
                new() { BookId = 14, GenreId = 25 }, //political
                //hitchhiker
                new() { BookId = 15, GenreId = 2 }, //science fiction
                new() { BookId = 15, GenreId = 3 }, //fantasy
                new() { BookId = 15, GenreId = 16 }, //comedy
            };
    }
}
