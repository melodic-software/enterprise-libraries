using System.Reflection;
using Enterprise.Applications.DI.Startup;
using Enterprise.Options.Core.Delegates;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using static Enterprise.Serilog.Startup.SerilogPreStartupLoggingService;

namespace Enterprise.Api.Startup.Options;

/// <summary>
/// Represents configuration options for the API, allowing for deferred and dynamic configuration during application startup.
/// </summary>
public partial class WebApiOptions
{
    /// <summary>
    /// Contains the options for web application that override all other configuration sources.
    /// </summary>
    public WebApplicationOptions WebApplicationOptions { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="WebApiOptions"/> with specified command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments provided to the application.</param>
    public WebApiOptions(string[] args)
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
    public void Configure<TOptions>(Configure<TOptions> configureOptions) where TOptions : class, new()
    {
        OptionsInstanceService.Instance.Configure(configureOptions);
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
