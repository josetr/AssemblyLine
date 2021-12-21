namespace AssemblyLine.Tests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TestDb : IDisposable
{
    private SqliteConnection _connection;
    private IDbContextFactory<AssemblyLineDbContext> _factory;

    public IDbContextFactory<AssemblyLineDbContext> Factory
    {
        get
        {
            if (_factory == null)
            {
                _connection = new SqliteConnection("datasource=:memory:");
                _connection.Open();
                var options = new DbContextOptionsBuilder<AssemblyLineDbContext>().UseSqlite(_connection).Options;
                _factory = new SimpleDbContextFactory<AssemblyLineDbContext>(() => new AssemblyLineDbContext(options));
                using var context = _factory.CreateDbContext();
                context.Database.EnsureCreated();
            }

            return _factory;
        }
    }

    public void Dispose() => _connection?.Close();
}
