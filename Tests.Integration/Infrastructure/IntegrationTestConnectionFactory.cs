using Microsoft.Data.Sqlite;
using Persistence;

namespace Tests.Integration.Infrastructure;

public class IntegrationTestConnectionFactory : IDisposable
{
    private const string CONNECTION_STRING = SqliteDbContextFactory<TodoDb>.DEFAULT_SQLITE_CONNECTION_STRING;
    private SqliteConnection? _connection;
    private bool _disposedValue;

    public void ResetConnection()
    {
        _connection?.Dispose();
        _connection = new SqliteConnection(CONNECTION_STRING);
        _connection.Open();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue && _connection != null)
        {
            if (disposing)
            {
                _connection.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
