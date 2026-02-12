namespace BookHub.Features.Authors.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorDbModel>
{
    public void Configure(EntityTypeBuilder<AuthorDbModel> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder
            .Property(a => a.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

        builder
            .Property(a => a.Biography)
            .IsRequired()
            .HasMaxLength(BiographyMaxLength);

        builder
            .Property(a => a.PenName)
            .HasMaxLength(PenNameMaxLength);

        builder
            .HasOne(a => a.Creator)
            .WithMany(u => u.Authors)
            .HasForeignKey(a => a.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);

        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Authors",
            "Data",
            "Seed",
            "authors_seed.json");

        Seeder.SeedFromJson(
           builder,
           path);
    }
}
