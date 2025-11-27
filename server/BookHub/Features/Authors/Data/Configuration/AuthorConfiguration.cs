namespace BookHub.Features.Authors.Data.Configuration;

using BookHub.Common;
using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorDbModel>
{
    public void Configure(EntityTypeBuilder<AuthorDbModel> builder)
        => Seeder.SeedFromJson(
            builder,
            FeatureNames.Authors.ToString(),
            "authors_seed.json");
}
