namespace BookHub.Features.Reviews.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewDbModel>
{
    // One live review per user and book (B-20). The service checks first, but the check is
    // read-then-write, so this index is what stops two concurrent creates; the resulting unique
    // violation maps to a 409. Soft-deleted reviews are excluded, so a user can review again
    // after deleting their review.
    public void Configure(EntityTypeBuilder<ReviewDbModel> builder)
        => builder
            .HasIndex(r => new { r.CreatorId, r.BookId })
            .IsUnique()
            .HasFilter("NOT \"IsDeleted\"");
}
