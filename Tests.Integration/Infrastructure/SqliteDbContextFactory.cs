using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Integration.Infrastructure;

public sealed class SqliteDbContextFactory<T> : IDisposable where T : DbContext
{
    public const string DEFAULT_SQLITE_CONNECTION_STRING =  "Data Source=MinimalAPIsTest;Mode=Memory;Cache=Shared";

    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<T> _options;
    private readonly object[] _dbContextParameters;

    public SqliteDbContextFactory(Action<DbContextOptionsBuilder<T>>? additionalOptions = null, 
        params object[] dbContextParameters)
    {
        _connection = new SqliteConnection(DEFAULT_SQLITE_CONNECTION_STRING);
        _connection.Open();

        var optionsBuilder = new DbContextOptionsBuilder<T>()
           .EnableSensitiveDataLogging()
           .UseSqlite(_connection);
        additionalOptions?.Invoke(optionsBuilder);
        _options = optionsBuilder.Options;

        _dbContextParameters = dbContextParameters;

        using var db = CreateDbContext();
        db.Database.EnsureCreated();
    }

    public T CreateDbContext()
    {
        try
        {
            return Activator.CreateInstance(typeof(T), new[] { _options }.Union(_dbContextParameters).ToArray()) as T ??
               throw new Exception($"Instance created of unexpected type {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to instantiate DbContext {typeof(T).Name} - did you forget to pass in any required constructor parameters?", ex);
        }
    }

    public T CreateDbContext(params object[] ctorParams)
    {
        try
        {
            return Activator.CreateInstance(typeof(T), new[] { _options }.Union(ctorParams).ToArray()) as T ??
                   throw new Exception($"Instance created not of expected type {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to instantiate DbContext {typeof(T).Name} - did you forget to pass in some required constructor parameters?", ex);
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
