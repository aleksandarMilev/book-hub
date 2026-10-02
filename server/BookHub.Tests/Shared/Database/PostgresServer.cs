namespace BookHub.Tests.Shared.Database;

using BookHub.Data;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSubstitute;
using Testcontainers.PostgreSql;

// One PostgreSQL container for the whole test run (xunit v2 has no assembly fixture, so it's a
// lazily started static). The real migrations are applied once to a template database, and each
// test gets its own database cloned from that template: same isolation as a fresh database per
// test, without re-running the migrations. Testcontainers' Ryuk removes the container at exit.
internal static class PostgresServer
{
    // Keep the image and the initdb arguments in sync with docker-compose.*.yml.
    private const string Image = "postgres:18-alpine";
    private const string InitDbArgs = "--encoding=UTF8 --locale-provider=icu --icu-locale=und";
    private const string TemplateDatabase = "bookhub_template";

    private static readonly SemaphoreSlim StartLock = new(1, 1);

    private static PostgreSqlContainer? container;

    public static async Task<TestDatabase> CreateDatabase()
    {
        await EnsureStarted();

        // Generated, never user input: DDL can't take parameters.
        var name = $"test_{Guid.NewGuid():N}";
        await ExecuteOnServer($"CREATE DATABASE \"{name}\" TEMPLATE \"{TemplateDatabase}\"");

        return new TestDatabase(name, ConnectionStringFor(name));
    }

    internal static async Task DropDatabase(string name)
    {
        NpgsqlConnection.ClearPool(new NpgsqlConnection(ConnectionStringFor(name)));
        await ExecuteOnServer($"DROP DATABASE IF EXISTS \"{name}\" WITH (FORCE)");
    }

    private static async Task EnsureStarted()
    {
        if (container is not null)
        {
            return;
        }

        await StartLock.WaitAsync();

        try
        {
            if (container is not null)
            {
                return;
            }

            // Durability settings off: the data is thrown away, and it halves the clone time.
            var newContainer = new PostgreSqlBuilder(Image)
                .WithEnvironment("POSTGRES_INITDB_ARGS", InitDbArgs)
                .WithCommand(
                    "-c", "fsync=off",
                    "-c", "synchronous_commit=off",
                    "-c", "full_page_writes=off")
                .Build();

            await newContainer.StartAsync();

            await CreateTemplate(newContainer.GetConnectionString());

            container = newContainer;
        }
        finally
        {
            StartLock.Release();
        }
    }

    private static async Task CreateTemplate(string serverConnectionString)
    {
        await using (var connection = new NpgsqlConnection(serverConnectionString))
        {
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                $"CREATE DATABASE \"{TemplateDatabase}\"",
                connection);

            await command.ExecuteNonQueryAsync();
        }

        var templateConnectionString = new NpgsqlConnectionStringBuilder(serverConnectionString)
        {
            Database = TemplateDatabase
        }.ConnectionString;

        var options = new DbContextOptionsBuilder<BookHubDbContext>()
            .UseNpgsql(templateConnectionString)
            .Options;

        await using (var data = new BookHubDbContext(options, Substitute.For<ICurrentUserService>()))
        {
            await data.Database.MigrateAsync();
        }

        // CREATE DATABASE ... TEMPLATE fails while any session is connected to the template.
        NpgsqlConnection.ClearAllPools();
    }

    private static async Task ExecuteOnServer(string sql)
    {
        await using var connection = new NpgsqlConnection(container!.GetConnectionString());
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private static string ConnectionStringFor(string database)
        => new NpgsqlConnectionStringBuilder(container!.GetConnectionString())
        {
            Database = database
        }.ConnectionString;
}
