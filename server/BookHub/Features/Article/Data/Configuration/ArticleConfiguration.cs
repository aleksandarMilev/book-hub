namespace BookHub.Features.Article.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using Seed;

    public class ArticleConfiguration : IEntityTypeConfiguration<ArticleDbModel>
    {
        public void Configure(EntityTypeBuilder<ArticleDbModel> builder)
            => builder.HasData(ArticleSeeder.Seed());
    }
}
