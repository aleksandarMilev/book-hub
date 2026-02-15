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

        builder
            .HasIndex(e => e.AuthorId)
            .IsUnique();

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
            .Property(e => e.RequestedById)
            .IsRequired();

        builder
            .HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
