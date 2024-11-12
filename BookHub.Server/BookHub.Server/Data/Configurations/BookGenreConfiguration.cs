namespace BookHub.Server.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using Seed;

    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
            => builder.HasData(BookGenreSeeder.Seed());
    }
}
