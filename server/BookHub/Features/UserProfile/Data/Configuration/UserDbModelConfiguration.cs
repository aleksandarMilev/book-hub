namespace BookHub.Features.UserProfile.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

using static Shared.Constants.Validation;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder
            .HasKey(p => p.UserId);

        builder
            .Property(p => p.UserId)
            .IsRequired();

        builder
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder
            .Property(p => p.ImagePath)
            .IsRequired();

        builder
            .Property(p => p.SocialMediaUrl)
            .HasMaxLength(UrlMaxLength);

        builder
            .Property(p => p.Biography)
            .HasMaxLength(BiographyMaxLength);

        builder
            .Property(p => p.IsPrivate)
            .IsRequired();

        builder
            .Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
