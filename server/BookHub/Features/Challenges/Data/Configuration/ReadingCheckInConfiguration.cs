namespace BookHub.Features.Challenges.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ReadingCheckInConfiguration : IEntityTypeConfiguration<ReadingCheckInDbModel>
{
    public void Configure(EntityTypeBuilder<ReadingCheckInDbModel> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(c => c.Date)
            .HasColumnType("date")
            .IsRequired();

        builder
            .HasOne(c => c.User)
            .WithMany(u => u.ReadingCheckIns)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(c => new { c.UserId, c.Date })
            .IsUnique();
    }
}
