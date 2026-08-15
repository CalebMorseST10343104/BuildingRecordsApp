using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Database;

internal sealed class SqliteTestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    private SqliteTestDatabase(SqliteConnection connection, BuildingContext context)
    {
        _connection = connection;
        Context = context;
    }

    public BuildingContext Context { get; }

    public static async Task<SqliteTestDatabase> CreateAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:;Foreign Keys=True");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<BuildingContext>().UseSqlite(connection).Options;
        var database = new SqliteTestDatabase(connection, new BuildingContext(options));
        await database.Context.Database.EnsureCreatedAsync();
        return database;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
