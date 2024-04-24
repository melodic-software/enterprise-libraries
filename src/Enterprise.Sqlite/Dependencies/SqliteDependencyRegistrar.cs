using Enterprise.Data.Relational;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Sqlite.Dependencies;

public static class SqliteDependencyRegistrar
{
    public static void RegisterSqliteConnectionFactory(this IServiceCollection services, string connectionStringConfigKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionStringConfigKey);

        services.AddSingleton(provider =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

            string connectionString = configuration.GetConnectionString(connectionStringConfigKey) ?? string.Empty;

            IDbConnectionFactory connectionFactory = new SqliteConnectionFactory(connectionString);

            return connectionFactory;
        });
    }
}