namespace BookHub.Features.Statistics.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class StatisticsConfiguration : IEntityTypeConfiguration<StatisticsRow>
{
    public void Configure(EntityTypeBuilder<StatisticsRow> builder)
        => builder
            .HasNoKey()
            .ToView(null);
}
