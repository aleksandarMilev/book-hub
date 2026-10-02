namespace BookHub.Features.Authors.Data.Configuration;

using Infrastructure.Extensions;
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
            .Property(a => a.BornAt)
            .HasColumnType("date");

        builder
            .Property(a => a.DiedAt)
            .HasColumnType("date");

        builder.HasSearchVector(
            nameof(AuthorDbModel.Name),
            nameof(AuthorDbModel.PenName));

        builder
            .HasOne(a => a.Creator)
            .WithMany(u => u.Authors)
            .HasForeignKey(a => a.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
