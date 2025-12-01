namespace BookHub.Features.Book.Data.Configuration;

using BookHub.Data.Seeder;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class BookConfiguration : IEntityTypeConfiguration<BookDbModel>
{
    public void Configure(EntityTypeBuilder<BookDbModel> builder)
        => Seeder.SeedFromJson(
            builder,
            FeatureNames.Book.ToString(),
            "books_seed.json");
}
