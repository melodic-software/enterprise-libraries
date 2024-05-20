using Enterprise.Monitoring.Health.Model;
using Enterprise.Monitoring.Health.Options;
using Enterprise.Options.Core.Singleton;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enterprise.Monitoring.Health.Config;

public static class HealthCheckConfigService
{
    // TODO: explore more here: https://app.pluralsight.com/library/courses/asp-dot-net-core-health-checks/table-of-contents

    private const string DefaultOpenIdConnectAuthorityName = "identity-provider";
    private const string IdentityServerAuthorityName = "identity-server";

    public static void ConfigureHealthChecks(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        HealthCheckConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<HealthCheckConfigOptions>(configuration, HealthCheckConfigOptions.ConfigSectionKey);

        IHealthChecksBuilder healthCheckBuilder = builder.Services.AddHealthChecks();

        if (!string.IsNullOrWhiteSpace(options.SqlConnectionString))
        {
            healthCheckBuilder.AddSqlServer(options.SqlConnectionString);
        }

        if (!string.IsNullOrWhiteSpace(options.PostgresConnectionString))
        {
            healthCheckBuilder.AddNpgSql(options.PostgresConnectionString);
        }

        RegisterIdentityProviderHealthCheck(options, healthCheckBuilder);

        if (!string.IsNullOrWhiteSpace(options.RedisConnectionString))
        {
            healthCheckBuilder.AddRedis(options.RedisConnectionString);
        }

        if (options.RabbitMqOptions != null)
        {
            healthCheckBuilder.AddRabbitMQ(checkOptions =>
            {
                checkOptions.Connection = options.RabbitMqOptions.Connection;
                checkOptions.ConnectionFactory = options.RabbitMqOptions.ConnectionFactory;
                checkOptions.ConnectionUri = options.RabbitMqOptions.ConnectionUri;
                checkOptions.RequestedConnectionTimeout = options.RabbitMqOptions.RequestedConnectionTimeout;
                checkOptions.Ssl = options.RabbitMqOptions.Ssl;
            });
        }

        foreach (UriGroup group in options.UrlGroup)
        {
            healthCheckBuilder.AddUrlGroup(group.Uri, group.HttpMethod, group.Name);
        }

        // This allows for application specific health checks like entity framework DB contexts, etc.
        // These are the additional health checks services that can be registered:
        // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        options.AddHealthChecks.Invoke(healthCheckBuilder);
    }

    public static void UseHealthChecks(this WebApplication app)
    {
        HealthCheckConfigOptions options = app.Services.GetRequiredService<IOptions<HealthCheckConfigOptions>>().Value;

        IEndpointConventionBuilder builder = app.MapHealthChecks(options.UrlPatternName, new()
        {
            // This will write JSON data in the HTTP response to the health check endpoint.
            // This comes from the AspNetCore.HealthChecks.UI.Client package.
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        if (options.AllowAnonymous)
        {
            // Any calls to the health check endpoint will not require authentication.
            builder.AllowAnonymous();
        }
    }

    private static void RegisterIdentityProviderHealthCheck(HealthCheckConfigOptions options, IHealthChecksBuilder healthCheckBuilder)
    {
        if (string.IsNullOrWhiteSpace(options.OpenIdConnectAuthority))
        {
            return;
        }

        string openIdConnectAuthorityName = options.OpenIdConnectAuthorityName?.Trim().ToLowerInvariant() ?? DefaultOpenIdConnectAuthorityName;

        if (openIdConnectAuthorityName == IdentityServerAuthorityName)
        {
            healthCheckBuilder.AddIdentityServer(new Uri(options.OpenIdConnectAuthority));
        }
        else
        {
            healthCheckBuilder.AddUrlGroup(new Uri(options.OpenIdConnectAuthority), HttpMethod.Get, openIdConnectAuthorityName);
        }
    }
}
