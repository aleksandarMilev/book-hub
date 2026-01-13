namespace BookHub.Features.Genres.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class GenreConfiguration : IEntityTypeConfiguration<GenreDbModel>
{
    public void Configure(EntityTypeBuilder<GenreDbModel> builder)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Genres",
            "Data",
            "Seed",
            "genres_seed.json");

        Seeder.SeedFromJson(
           builder,
           path);
    }
}
