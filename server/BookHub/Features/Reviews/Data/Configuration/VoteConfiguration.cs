namespace BookHub.Features.Review.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class VoteConfiguration : IEntityTypeConfiguration<VoteDbModel>
{
    public void Configure(EntityTypeBuilder<VoteDbModel> builder)
        => builder
            .HasIndex(v => new { v.ReviewId, v.CreatorId })
            .IsUnique();
}
