using System.Data;

namespace Enterprise.Data.Relational;

public interface IDbConnectionFactory
{
    /// <summary>
    /// Create a database connection.
    /// </summary>
    /// <returns></returns>
    Task<IDbConnection> CreateConnectionAsync();
}