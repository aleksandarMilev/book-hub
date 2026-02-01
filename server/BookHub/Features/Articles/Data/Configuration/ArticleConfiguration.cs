namespace BookHub.Features.Articles.Data.Configuration;

using BookHub.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Common.Constants.Validation;
using static Shared.Constants.Validation;

public class ArticleConfiguration : IEntityTypeConfiguration<ArticleDbModel>
{
    public void Configure(EntityTypeBuilder<ArticleDbModel> builder)
    {
        builder
          .Property(a => a.Title)
          .IsRequired()
          .HasMaxLength(TitleMaxLength);

        builder
            .Property(a => a.Introduction)
            .IsRequired()
            .HasMaxLength(IntroductionMaxLength);

        builder
            .Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(ContentMaxLength);

        builder
            .Property(a => a.ImagePath)
            .IsRequired()
            .HasMaxLength(ImagePathMaxLength);

        builder
            .Property(a => a.Views)
            .HasDefaultValue(0);

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
