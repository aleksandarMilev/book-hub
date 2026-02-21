namespace BookHub.Features.Articles.Data.Configuration;

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
            .HasKey(a => a.Id);

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
    }
}
