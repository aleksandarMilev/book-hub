namespace BookHub.Features.Genre.Data.Configuration;

using BookHub.Common;
using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class GenreConfiguration : IEntityTypeConfiguration<GenreDbModel>
{
    public void Configure(EntityTypeBuilder<GenreDbModel> builder)
        => Seeder.SeedFromJson(
            builder,
            FeatureNames.Genres.ToString(),
            "genres_seed.json");
}
