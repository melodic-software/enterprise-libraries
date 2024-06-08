using System.Reflection;
using Enterprise.Api.Startup.Options.Delegates;
using Enterprise.Applications.DI.Startup;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Enterprise.Serilog.Startup.SerilogPreStartupLoggingService;

namespace Enterprise.Api.Startup.Options;

/// <summary>
/// Represents configuration options for the API, allowing for deferred and dynamic configuration during application startup.
/// </summary>
public class ApiConfigOptions
{
    /// <summary>
    /// Holds deferred configuration actions that are to be executed later in the application lifecycle.
    /// </summary>
    internal List<DeferredConfiguration> DeferredConfigurations = [];

    /// <summary>
    /// Contains the options for web application that override all other configuration sources.
    /// </summary>
    public WebApplicationOptions WebApplicationOptions { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ApiConfigOptions"/> with specified command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments provided to the application.</param>
    public ApiConfigOptions(string[] args)
    {
        EnsureSharedFrameworkAssemblyIsLoaded();
        ConfigurePreStartupLogger();
        
        WebApplicationOptions = new WebApplicationOptions
        {
            Args = args
        };
    }

    /// <summary>
    /// Configures options of a specific type.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="configureOptions">The configuration action to be applied.</param>
    public void Configure<TOptions>(Action<TOptions> configureOptions) where TOptions : class, new()
    {
        // TODO: Use named delegate "Configure<T>".
        OptionsInstanceService.Instance.Configure(configureOptions);
    }

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

    /// <summary>
    /// Ensures the shared framework assembly directory is loaded.
    /// This is required if no AutoLoad assembly attribute is defined.
    /// </summary>
    private static void EnsureSharedFrameworkAssemblyIsLoaded()
    {
        Type controllerType = typeof(Controller);
        Assembly controllerAssembly = controllerType.Assembly;
        string? sharedFrameworkAssemblyDirectory = Path.GetDirectoryName(controllerAssembly.Location);
        SharedFrameworkAssemblyService.Instance.AddSharedDirectory(sharedFrameworkAssemblyDirectory);
    }
}
