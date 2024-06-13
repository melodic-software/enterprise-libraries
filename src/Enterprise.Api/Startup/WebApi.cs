using System.Diagnostics;
using Enterprise.Api.Diagnostics;
using Enterprise.Api.Startup.Delegates;
using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Options;
using Enterprise.Applications.DI.ServiceCollection;
using Enterprise.Logging.Core.Loggers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Startup;

/// <summary>
/// Provides methods to bootstrap and run the web API.
/// This includes configuring services, request pipeline middleware, and other startup tasks.
/// </summary>
public static class WebApi
{
    /// <summary>
    /// Bootstraps the web API by configuring services and the request pipeline, and starts the application.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task RunAsync(string[] args) => await RunAsync(args, (_, _) => { });

    /// <summary>
    /// Bootstraps the web API by configuring services and the request pipeline, and starts the application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <param name="configure">A delegate to configure <see cref="WebApiOptions"/> and handle <see cref="WebApiConfigEvents"/>.</param>
    public static async Task RunAsync(string[] args, ConfigureApi? configure)
    {
        var stopwatch = Stopwatch.StartNew();

        var options = new WebApiOptions(args);
        var events = new WebApiConfigEvents();

        try
        {
            PreStartupLogger.Instance.LogInformation("Beginning configuration.");

            // This is the primary hook for applications to configure the API.
            // This must be placed first in case the application decides to wire up event handlers.
            configure?.Invoke(options, events);

            // This is the first extensibility hook that allows for pre-configuration.
            await events.RaiseConfigurationStarted(args);

            WebApplicationBuilder builder = await CreateBuilderAsync(options, events);

            // Execute deferred configurations with the actual configuration and environment.
            PreStartupLogger.Instance.LogInformation("Executing deferred option configuration.");
            options.ExecuteDeferredConfigurations(builder.Configuration, builder.Environment);
            PreStartupLogger.Instance.LogInformation("Deferred option configuration complete.");

            await AddServicesAsync(builder, events);
            WebApplication app = await BuildApplicationAsync(builder, events);

            await ConfigureRequestPipelineAsync(app, events);

            app.Logger.LogInformation("API configuration is complete.");
            await events.RaiseConfigurationCompleted();

            RegisterLifetimeEventHandlers(app, events, stopwatch);

            await RunApplicationAsync(app);
        }
        catch (Exception ex)
        {
            PreStartupLogger.Instance.LogError(ex, "An error has occurred during API configuration.");
            await events.RaiseConfigurationErrorOccurred(ex);
            throw;
        }
    }

    /// <summary>
    /// Create the WebApplicationBuilder.
    /// This calls into the default builder creation method which registers logging providers, and other service defaults.
    /// https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebHost.cs#L155
    /// </summary>
    /// <param name="options"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    private static async Task<WebApplicationBuilder> CreateBuilderAsync(WebApiOptions options, WebApiConfigEvents events)
    {
        PreStartupLogger.Instance.LogInformation("Creating the WebApplicationBuilder.");
        WebApplicationBuilder builder = WebApplication.CreateBuilder(options.WebApplicationOptions);
        await events.RaiseBuilderCreated(builder);
        PreStartupLogger.Instance.LogInformation("WebApplicationBuilder created.");
        return builder;
    }

    /// <summary>
    /// Add services to the container.
    /// This includes framework services, and application specific services.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="events"></param>
    private static async Task AddServicesAsync(WebApplicationBuilder builder, WebApiConfigEvents events)
    {
        PreStartupLogger.Instance.LogInformation("Adding services to the DI container.");
        builder.ConfigureServices();
        PreStartupLogger.Instance.LogInformation("Services have been registered with the DI container.");
        await events.RaiseServicesConfigured(builder);
    }

    /// <summary>
    /// Build the application.
    /// All services must be registered before this is called.
    /// Any attempts to add additional services will result in an exception.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    private static async Task<WebApplication> BuildApplicationAsync(WebApplicationBuilder builder, WebApiConfigEvents events)
    {
        PreStartupLogger.Instance.LogInformation("Building the application.");

        builder.InsertAnalysisStartupFilter();

        // We have to add this right before we build the application.
        // This captures a snapshot of all registered services up to this point.
        // Once build is called, the service collection is transformed to a service provider,
        // at which point no new services can be added to the DI container.
        builder.AddServiceDescriptorRegistry();

        WebApplication app = builder.Build();

        PreStartupLogger.Instance.SetLogger(app.Logger);
        app.Logger.LogInformation("Application has been built.");

        await events.RaiseWebApplicationBuilt(app);

        return app;
    }

    /// <summary>
    /// Configure the HTTP request (middleware) pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="events"></param>
    /// <returns></returns>
    private static async Task ConfigureRequestPipelineAsync(WebApplication app, WebApiConfigEvents events)
    {
        app.Logger.LogInformation("Configuring the request pipeline.");
        app.ConfigureRequestPipeline();
        app.Logger.LogInformation("Request pipeline has been configured.");
        await events.RaiseRequestPipelineConfigured(app);
    }

    private static void RegisterLifetimeEventHandlers(WebApplication app, WebApiConfigEvents events, Stopwatch stopwatch)
    {
        app.Logger.LogInformation("Registering application lifetime event handlers.");

        IHostApplicationLifetime lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

        lifetime.ApplicationStarted.Register(() =>
        {
            stopwatch.Stop();

            app.Logger.LogInformation(
                "Application started. Total startup time: {ElapsedMilliseconds}ms",
                stopwatch.ElapsedMilliseconds
            );
        });

        lifetime.ApplicationStopping.Register(() =>
        {
            app.Logger.LogInformation("Application is stopping.");
        });

        lifetime.ApplicationStopped.Register(() =>
        {
            app.Logger.LogInformation("Application has stopped.");
            events.ClearHandlers();
        });

        app.Logger.LogInformation("Application lifetime event handler registration complete.");
    }

    private static async Task RunApplicationAsync(WebApplication app)
    {
        using IDisposable? disposable = MiddlewareAnalysisConfigService.GetDiagnosticListener(app);
        app.Logger.LogInformation("Running the application.");
        await app.RunAsync();
    }
}
