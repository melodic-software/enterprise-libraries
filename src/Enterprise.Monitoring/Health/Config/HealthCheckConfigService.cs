﻿using Enterprise.Monitoring.Health.Model;
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

    public static void ConfigureHealthChecks(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        HealthCheckConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<HealthCheckConfigOptions>(configuration, HealthCheckConfigOptions.ConfigSectionKey);

        IHealthChecksBuilder healthCheckBuilder = builder.Services.AddHealthChecks();

        if (!string.IsNullOrWhiteSpace(options.OpenIdConnectAuthority))
            healthCheckBuilder.AddIdentityServer(new Uri(options.OpenIdConnectAuthority));

        if (!string.IsNullOrWhiteSpace(options.RedisConnectionString))
            healthCheckBuilder.AddRedis(options.RedisConnectionString);

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

        if (!string.IsNullOrWhiteSpace(options.SqlConnectionString))
            healthCheckBuilder.AddSqlServer(options.SqlConnectionString);

        foreach (UriGroup group in options.UrlGroup)
            healthCheckBuilder.AddUrlGroup(new Uri(group.Uri), group.HttpMethod, group.Name);

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
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        if (options.AllowAnonymous)
        {
            // any calls to the health check endpoint will not require authentication
            builder.AllowAnonymous();
        }
    }
}