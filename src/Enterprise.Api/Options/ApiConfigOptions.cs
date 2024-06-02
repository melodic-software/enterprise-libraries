using Enterprise.Api.Events;
using Enterprise.Applications.DI.Startup;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Serilog.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Enterprise.Api.Options;

/// <summary>
/// Represents configuration options for the API, allowing for deferred and dynamic configuration during application startup.
/// </summary>
public class ApiConfigOptions
{
    /// <summary>
    /// Holds deferred configuration actions that are to be executed later in the application lifecycle.
    /// </summary>
    internal List<Action<IConfiguration, IWebHostEnvironment>> DeferredConfigurations = [];

    /// <summary>
    /// Contains the options for web application that override all other configuration sources.
    /// </summary>
    public WebApplicationOptions WebApplicationOptions { get; private set; }

    /// <summary>
    /// Allows for wiring up handlers to specific lifecycle events in the API configuration.
    /// </summary>
    public ApiConfigEvents Events { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ApiConfigOptions"/> with specified command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments provided to the application.</param>
    public ApiConfigOptions(string[] args)
    {
        // Ensures the shared framework assembly directory is loaded, required if no AutoLoad assembly attribute is defined.
        SharedFrameworkAssemblyService.Instance.AddSharedDirectory(Path.GetDirectoryName(typeof(Controller).Assembly.Location));

        // Replaces the pre-startup logger with a Serilog implementation until the Microsoft ILogger instance is configured.
        SerilogPreStartupLoggingService.ConfigurePreStartupLogger();

        WebApplicationOptions = new WebApplicationOptions
        {
            Args = args
        };
    }

    /// <summary>
    /// Configures the event handlers for specific application events.
    /// Use multicast delegates for adding handlers to ensure all registered handlers are invoked.
    /// </summary>
    /// <param name="configureHandlers">An action that configures the event handlers.</param>
    public void ConfigureEventHandlers(Action<ApiConfigEvents> configureHandlers)
    {
        configureHandlers.Invoke(Events);
    }

    /// <summary>
    /// Configures options of a specific type.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="configureOptions">The configuration action to be applied.</param>
    public void Configure<TOptions>(Action<TOptions> configureOptions) where TOptions : class, new()
    {
        OptionsInstanceService.Instance.Configure(configureOptions);
    }

    /// <summary>
    /// Configures options of a specific type with access to the configuration and web hosting environment.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="configureOptions">The configuration action to be applied that includes access to the application's configuration and environment.</param>
    public void Configure<TOptions>(Action<TOptions, IConfiguration, IWebHostEnvironment> configureOptions)
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
        foreach (Action<IConfiguration, IWebHostEnvironment> deferredConfiguration in DeferredConfigurations)
        {
            deferredConfiguration(configuration, environment);
        }
    }
}
