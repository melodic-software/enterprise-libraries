namespace Enterprise.Sqlite.Constants;

public static class ConnectionStrings
{
    /// <summary>
    /// This connection string creates a truly temporary database for the duration of a single connection,
    /// without sharing capability between different connections.
    /// </summary>
    public const string InMemory = "Data Source=:memory:";

    /// <summary>
    /// This connection string creates a temporary in-memory database that can be shared across multiple connections.
    /// This makes it more suitable for scenarios where data sharing between connections is required.
    /// </summary>
    public const string InMemoryFile = "DataSource=file:inmem?mode=memory&cache=shared";
}