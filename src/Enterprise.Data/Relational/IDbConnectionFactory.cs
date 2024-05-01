using System.Data;

namespace Enterprise.Data.Relational;

public interface IDbConnectionFactory
{
    /// <summary>
    /// Open a database connection.
    /// </summary>
    /// <returns></returns>
    ValueTask<IDbConnection> OpenConnectionAsync();
}
