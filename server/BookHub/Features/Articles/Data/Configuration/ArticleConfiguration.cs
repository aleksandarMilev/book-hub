namespace BookHub.Features.Article.Data.Configuration;

using BookHub.Common;
using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ArticleConfiguration : IEntityTypeConfiguration<ArticleDbModel>
{
    public void Configure(EntityTypeBuilder<ArticleDbModel> builder)
        => Seeder.SeedFromJson(
            builder,
            FeatureNames.Articles.ToString(),
            "articles_seed.json");
}
