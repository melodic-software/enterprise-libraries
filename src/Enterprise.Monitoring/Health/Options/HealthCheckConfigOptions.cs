using Enterprise.Monitoring.Health.Model;
using HealthChecks.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Monitoring.Health.Options;

public class HealthCheckConfigOptions
{
    public const string ConfigSectionKey = "HealthCheck";

    /// <summary>
    /// This is the path / resource that will be exposed
    /// </summary>
    public string UrlPatternName { get; set; } = "health-checks";

    /// <summary>
    /// If enabled, calls to the health check endpoint will not require authentication.
    /// </summary>
    public bool AllowAnonymous { get; set; } = true;

    /// <summary>
    /// If provided, a health check will be enabled for the OpenIdConnect provider.
    /// This is typically a security token service like "IdentityServer".
    /// </summary>
    public string? OpenIdConnectAuthority { get; set; } = null;

    /// <summary>
    /// If provided, a health check will be enabled for Redis.
    /// </summary>
    public string? RedisConnectionString { get; set; }

    /// <summary>
    /// The options for configuring a RabbitMQ health check.
    /// </summary>
    public RabbitMQHealthCheckOptions? RabbitMqOptions { get; set; }
    
    /// <summary>
    /// If provided, a SQL Server health check will be enabled.
    /// </summary>
    public string? SqlConnectionString { get; set; }

    /// <summary>
    /// When populated, a health check will be added for each URI.
    /// </summary>
    public List<UriGroup> UrlGroup { get; set; } = [];

    /// <summary>
    /// Allows for custom registrations of health checks specific to the executing application.
    /// These can be entity framework DB contexts, SQL server databases, etc.
    /// </summary>
    public Action<IHealthChecksBuilder> AddHealthChecks { get; set; } = _ => { };
}