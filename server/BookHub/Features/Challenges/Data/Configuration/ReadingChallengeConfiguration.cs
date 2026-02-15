namespace BookHub.Features.Challenges.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ReadingChallengeConfiguration : IEntityTypeConfiguration<ReadingChallengeDbModel>
{
    public void Configure(EntityTypeBuilder<ReadingChallengeDbModel> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(c => c.GoalType)
            .IsRequired();

        builder
            .HasOne(c => c.User)
            .WithMany(u => u.ReadingChallenges)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(c => new { c.UserId, c.Year })
            .IsUnique();
    }
}
