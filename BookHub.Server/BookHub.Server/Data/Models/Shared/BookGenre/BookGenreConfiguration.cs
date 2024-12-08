namespace BookHub.Server.Data.Models.Shared.BookGenre
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
            => builder.HasData(BookGenreSeeder.Seed());
    }
}
