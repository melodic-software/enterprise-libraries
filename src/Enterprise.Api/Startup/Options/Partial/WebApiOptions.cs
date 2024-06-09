using Enterprise.Api.Startup.Options.Delegates;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Enterprise.Api.Startup.Options;

public partial class WebApiOptions
{
    /// <summary>
    /// Holds deferred configuration actions that are to be executed later in the application lifecycle.
    /// </summary>
    internal List<DeferredConfiguration> DeferredConfigurations = [];

    /// <summary>
    /// Configures options of a specific type with access to the configuration and web hosting environment.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="configureOptions">The configuration to be applied that includes access to the application's configuration and environment.</param>
    public void Configure<TOptions>(Configure<TOptions> configureOptions)
        where TOptions : class, new()
    {
        DeferredConfigurations.Add((config, env) =>
        {
            OptionsInstanceService.Instance.Configure<TOptions>(options =>
            {
                configureOptions(options, config, env);
            });
        });
    }

    /// <summary>
    /// Executes all deferred configurations using the provided configuration and web hosting environment.
    /// </summary>
    /// <param name="configuration">The application's configuration.</param>
    /// <param name="environment">The web hosting environment.</param>
    internal void ExecuteDeferredConfigurations(IConfiguration configuration, IWebHostEnvironment environment)
    {
        foreach (DeferredConfiguration deferredConfiguration in DeferredConfigurations)
        {
            deferredConfiguration(configuration, environment);
        }
    }
}
