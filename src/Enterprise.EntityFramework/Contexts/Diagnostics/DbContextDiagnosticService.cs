using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Contexts.Diagnostics;

public class DbContextDiagnosticService<T> where T : DbContext
{
    private readonly T _dbContext;
    private readonly ILogger<DbContextDiagnosticService<T>> _logger;

    public DbContextDiagnosticService(T dbContext, ILogger<DbContextDiagnosticService<T>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void LogDbContextInfo()
    {
        // Get the connection string.
        string? connectionString = _dbContext.Database.GetConnectionString();
        _logger.LogInformation("Connection String: {ConnectionString}", connectionString);

        // Get the database provider being used (e.g., SQL Server, SQLite, etc.).
        string? databaseProvider = _dbContext.Database.ProviderName;
        _logger.LogInformation("Database Provider: {DatabaseProvider}", databaseProvider);

        // Check if the context is connected to the database.
        bool isConnected = _dbContext.Database.CanConnect();
        _logger.LogInformation("Can Connect to Database: {IsConnected}", isConnected);

        // Log the detailed database context info including migration history.
        IEnumerable<string> migrations = _dbContext.Database.GetMigrations();
        _logger.LogInformation("Migrations: {Migrations}", string.Join(", ", migrations));

        IEnumerable<string> pendingMigrations = _dbContext.Database.GetPendingMigrations();
        _logger.LogInformation("Pending Migrations: {PendingMigrations}", string.Join(", ", pendingMigrations));

        IEnumerable<string> appliedMigrations = _dbContext.Database.GetAppliedMigrations();
        _logger.LogInformation("Applied Migrations: {AppliedMigrations}", string.Join(", ", appliedMigrations));
    }
}
