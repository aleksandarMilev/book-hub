namespace BookHub.Tests.Shared.Database;

// A database cloned from the migrated template. Disposing it drops the database.
public sealed class TestDatabase(
    string name,
    string connectionString) : IAsyncDisposable
{
    public string ConnectionString { get; } = connectionString;

    public async ValueTask DisposeAsync()
        => await PostgresServer.DropDatabase(name);
}
