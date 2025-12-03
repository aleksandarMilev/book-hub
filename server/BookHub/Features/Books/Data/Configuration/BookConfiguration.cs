namespace BookHub.Features.Book.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class BookConfiguration : IEntityTypeConfiguration<BookDbModel>
{
    public void Configure(EntityTypeBuilder<BookDbModel> builder)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Books",
            "Data",
            "Seed",
            "books_seed.json");

        Seeder.SeedFromJson(
           builder,
           path);
    }
}
