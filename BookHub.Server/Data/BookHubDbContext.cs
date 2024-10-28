namespace BookHub.Server.Data
{
    using BookHub.Server.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BookHubDbContext : IdentityDbContext<User>
    {
        public BookHubDbContext(DbContextOptions<BookHubDbContext> options)
            : base(options)
        {
        }
    }
}
