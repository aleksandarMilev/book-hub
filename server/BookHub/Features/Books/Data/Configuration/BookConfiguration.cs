namespace BookHub.Features.Books.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public sealed class BookConfiguration : IEntityTypeConfiguration<BookDbModel>
{
    public void Configure(EntityTypeBuilder<BookDbModel> builder)
    {
        builder
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(TitleMaxLength);

        builder
            .Property(b => b.ShortDescription)
            .IsRequired()
            .HasMaxLength(ShortDescriptionMaxLength);

        builder
            .Property(b => b.LongDescription)
            .IsRequired()
            .HasMaxLength(LongDescriptionMaxLength);

        builder
            .Property(b => b.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

        builder
            .HasOne(b => b.Author)
            .WithMany()
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(b => b.Creator)
            .WithMany()
            .HasForeignKey(b => b.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasMany(b => b.BooksGenres)
            .WithOne(bg => bg.Book)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(b => b.ReadingLists)
            .WithOne(rl => rl.Book)
            .HasForeignKey(rl => rl.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Books",
            "Data",
            "Seed",
            "books_seed.json");

        Seeder.SeedFromJson(builder, path);
    }
}
