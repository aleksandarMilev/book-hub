namespace BookHub.Features.Authors.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public class AuthorEditConfiguration : IEntityTypeConfiguration<AuthorEditDbModel>
{
    public void Configure(EntityTypeBuilder<AuthorEditDbModel> builder)
    {
        builder
            .HasKey(e => e.Id);

        // One live pending edit per author. Approved and rejected edits are soft-deleted,
        // so they must not count, or the next edit would get a 409 (B-26).
        builder
            .HasIndex(e => e.AuthorId)
            .IsUnique()
            .HasFilter("NOT \"IsDeleted\"");

        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder
            .Property(e => e.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

        builder
            .Property(e => e.Biography)
            .IsRequired()
            .HasMaxLength(BiographyMaxLength);

        builder
            .Property(e => e.PenName)
            .HasMaxLength(PenNameMaxLength);

        builder
            .Property(e => e.BornAt)
            .HasColumnType("date");

        builder
            .Property(e => e.DiedAt)
            .HasColumnType("date");

        builder
            .Property(e => e.RequestedById)
            .IsRequired();

        builder
            .HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
