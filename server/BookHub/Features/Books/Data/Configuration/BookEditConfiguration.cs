namespace BookHub.Features.Books.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public sealed class BookEditConfiguration : IEntityTypeConfiguration<BookEditDbModel>
{
    public void Configure(EntityTypeBuilder<BookEditDbModel> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .HasIndex(e => e.BookId)
            .IsUnique();

        builder
            .Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(TitleMaxLength);

        builder
            .Property(e => e.ShortDescription)
            .IsRequired()
            .HasMaxLength(ShortDescriptionMaxLength);

        builder
            .Property(e => e.LongDescription)
            .IsRequired()
            .HasMaxLength(LongDescriptionMaxLength);

        builder
            .Property(e => e.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

        builder
            .Property(e => e.RequestedById)
            .IsRequired();

        builder
            .Property(e => e.GenresJson)
            .IsRequired();

        builder
            .HasOne(e => e.Book)
            .WithMany()
            .HasForeignKey(e => e.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
