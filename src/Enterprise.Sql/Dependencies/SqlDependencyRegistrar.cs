using Enterprise.Data.Relational;
using Enterprise.Sql.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Sql.Dependencies;

public static class SqlDependencyRegistrar
{
    public static void RegisterSqlServerConnectionFactory(this IServiceCollection services, string connectionStringConfigKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionStringConfigKey);

        services.AddSingleton(provider =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

            string connectionString = configuration.GetConnectionString(connectionStringConfigKey) ?? string.Empty;

            IDbConnectionFactory connectionFactory = new SqlConnectionFactory(connectionString);

            return connectionFactory;
        });
    }
}