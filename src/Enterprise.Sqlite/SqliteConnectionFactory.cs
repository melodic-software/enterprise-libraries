using Enterprise.Data.Relational;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Enterprise.Sqlite;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public SqliteConnectionFactory(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}