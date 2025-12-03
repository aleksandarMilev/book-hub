namespace BookHub.Data.Models.Shared.BookGenre.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenreDbModel>
{
    public void Configure(EntityTypeBuilder<BookGenreDbModel> builder)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "Models",
            "Shared",
            "BookGenre",
            "Seed",
            "book_genre_seed.json");

        Seeder.SeedFromJson(
           builder,
           path);
    }
}
