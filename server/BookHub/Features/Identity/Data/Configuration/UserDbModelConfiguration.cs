namespace BookHub.Features.Identity.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using UserProfile.Data.Models;

public sealed class UserDbModelConfiguration : IEntityTypeConfiguration<UserDbModel>
{
    public void Configure(EntityTypeBuilder<UserDbModel> builder)
    {
        builder
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(u => !u.IsDeleted);

        builder
            .HasIndex(u => u.IsDeleted);
    }
}
