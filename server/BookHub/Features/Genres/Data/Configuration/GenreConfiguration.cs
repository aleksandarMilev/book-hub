namespace BookHub.Features.Genres.Data.Configuration;

using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Books.Shared.Constants.Genres;
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

        builder.HasSearchVector(nameof(GenreDbModel.Name));

        // Books created without genres get "Other", so it must exist on a fresh database.
        // The values match DataImporter's genres.json, which skips IDs that already exist.
        builder.HasData(new GenreDbModel
        {
            Id = OtherGenreId,
            Name = "Other",
            Description = "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions that don’t fit neatly into predefined genres.",
            ImagePath = "/images/genres/other.png",
            CreatedOn = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        });
    }
}
