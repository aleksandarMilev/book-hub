namespace BookHub.Tests.Shared.Database;

using BookHub.Data;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

// A database cloned from the migrated template. Disposing it drops the database.
public sealed class TestDatabase(
    string name,
    string connectionString) : IAsyncDisposable
{
    public string ConnectionString { get; } = connectionString;

    public BookHubDbContext CreateContext(ICurrentUserService currentUserService)
    {
        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseNpgsql(this.ConnectionString)
            .Options;

        return new BookHubDbContext(options, currentUserService);
    }

    public async ValueTask DisposeAsync()
        => await PostgresServer.DropDatabase(name);
}
