namespace BookHub.Features.Article.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ArticleConfiguration : IEntityTypeConfiguration<ArticleDbModel>
{
    public void Configure(EntityTypeBuilder<ArticleDbModel> builder)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features",
            "Articles",
            "Data",
            "Seed",
            "articles_seed.json");

         Seeder.SeedFromJson(
            builder,
            path);
    }
}
