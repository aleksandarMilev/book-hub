namespace BookHub.Server.Data.Seed
{
    using Models;

    public static class GenresSeeder
    {
        public static Genre[] Seed()
            => new Genre[]
            {
                new() { Id = 1, Name = "Horror" },
                new() { Id = 2, Name = "Science Fiction" },
                new() { Id = 3, Name = "Fantasy" },
                new() { Id = 4, Name = "Mystery" },
                new() { Id = 5, Name = "Romance" },
                new() { Id = 6, Name = "Thriller" },
                new() { Id = 7, Name = "Adventure" },
                new() { Id = 8, Name = "Historical" },
                new() { Id = 9, Name = "Biography" },
                new() { Id = 10, Name = "Self-help" },
                new() { Id = 11, Name = "Non-fiction" },
                new() { Id = 12, Name = "Poetry" },
                new() { Id = 13, Name = "Drama" },
                new() { Id = 14, Name = "Children's" },
                new() { Id = 15, Name = "Young Adult" },
                new() { Id = 16, Name = "Comedy" },
                new() { Id = 17, Name = "Graphic Novel" },
                new() { Id = 18, Name = "Other" }
            };
    }
}
