namespace BookHub.Features.Genres.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public class GenreConfiguration : IEntityTypeConfiguration<GenreDbModel>
{
    public void Configure(EntityTypeBuilder<GenreDbModel> builder)
    {
        builder
            .HasKey(g => g.Id);

        builder
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder
            .Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(DescriptionMaxLength);

        builder
            .Property(g => g.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

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
