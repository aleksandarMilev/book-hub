namespace BookHub.Features.Authors.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorDbModel>
{
    public void Configure(EntityTypeBuilder<AuthorDbModel> builder)
    {
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
