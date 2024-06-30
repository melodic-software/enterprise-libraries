using System.Diagnostics;
using System.Text.Json;
using Enterprise.Api.Diagnostics;
using Enterprise.Api.Startup.Events;
using Enterprise.Api.Startup.Options;
using Enterprise.Applications.DI.ServiceCollection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.ModularMonoliths.Configuration;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Enterprise.Api.Startup.Errors.IgnoredExceptionService;

namespace Enterprise.Api.Startup;

/// <summary>
/// Provides methods to bootstrap and run the web API.
/// This includes configuring services, request pipeline middleware, and other startup tasks.
/// </summary>
public static class WebApi
{
    private static WebApiConfigEvents Events { get; }
    private static WebApiOptions Options { get; set; }
    private static Stopwatch Stopwatch { get; }

    static WebApi()
    {
        Events = new WebApiConfigEvents();
        Stopwatch = new Stopwatch();
    }

    /// <summary>
    /// Bootstraps the web API by configuring services and the request pipeline, and starts the application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    public static async Task RunAsync(string[] args)
    {
        try
        {
            Stopwatch.Start();

            Options = new WebApiOptions(args);

            PreStartupLogger.Instance.LogInformation("Beginning configuration.");

            // These have to be in place before the first lifecycle event is raised.
            // This allows the application to add additional configuration at specific points in the lifecycle.
            WebApiConfigEventHandlerRegistrar.RegisterHandlers(Events);

            // This is the first extensibility hook that allows for pre-configuration.
            await Events.RaiseConfigurationStarted(args);

            WebApplicationBuilder builder = await CreateBuilderAsync();
            WebApiOptionConfigurer.Configure(Options, builder.Configuration);

            // Execute deferred configurations with the actual configuration and environment.
            PreStartupLogger.Instance.LogInformation("Executing deferred option configuration.");
            Options.ExecuteDeferredConfigurations(builder.Configuration, builder.Environment);
            PreStartupLogger.Instance.LogInformation("Deferred option configuration complete.");

            await AddServicesAsync(builder);
            WebApplication app = await BuildApplicationAsync(builder);

            await ConfigureRequestPipelineAsync(app);

            app.Logger.LogInformation("API configuration is complete.");
            await Events.RaiseConfigurationCompleted();

            RegisterLifetimeEventHandlers(app);

            await RunApplicationAsync(app);
        }
        catch (Exception ex) when (ExceptionShouldNotBeIgnored(ex))
        {
            PreStartupLogger.Instance.LogError(ex, "An error has occurred during API configuration.");
            await Events.RaiseConfigurationErrorOccurred(ex);
            throw;
        }
    }

    /// <summary>
    /// Create the WebApplicationBuilder.
    /// This calls into the default builder creation method which registers logging providers, and other service defaults.
    /// https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebHost.cs#L155
    /// </summary>
    /// <returns></returns>
    private static async Task<WebApplicationBuilder> CreateBuilderAsync()
    {
        PreStartupLogger.Instance.LogInformation("Creating the WebApplicationBuilder.");
        WebApplicationBuilder builder = WebApplication.CreateBuilder(Options.WebApplicationOptions);
        builder.RegisterModuleConfig();
        await Events.RaiseBuilderCreated(builder);
        PreStartupLogger.Instance.LogInformation("WebApplicationBuilder created.");
        return builder;
    }

    /// <summary>
    /// Add services to the container.
    /// This includes framework services, and application specific services.
    /// </summary>
    /// <param name="builder"></param>
    private static async Task AddServicesAsync(WebApplicationBuilder builder)
    {
        PreStartupLogger.Instance.LogInformation("Adding services to the DI container.");
        builder.ConfigureServices();
        PreStartupLogger.Instance.LogInformation("Services have been registered with the DI container.");
        await Events.RaiseServicesConfigured(builder);
    }

    /// <summary>
    /// Build the application.
    /// All services must be registered before this is called.
    /// Any attempts to add additional services will result in an exception.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static async Task<WebApplication> BuildApplicationAsync(WebApplicationBuilder builder)
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

        await Events.RaiseWebApplicationBuilt(app);

        return app;
    }

    /// <summary>
    /// Configure the HTTP request (middleware) pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    private static async Task ConfigureRequestPipelineAsync(WebApplication app)
    {
        app.Logger.LogInformation("Configuring the request pipeline.");
        app.ConfigureRequestPipeline();
        MapApplicationUptimeResource(app);
        app.Logger.LogInformation("Request pipeline has been configured.");
        await Events.RaiseRequestPipelineConfigured(app);
    }

    private static void MapApplicationUptimeResource(WebApplication app)
    {
        // TODO: Add configuration around what this path is.
        // TODO: Add constants for route.
        app.MapGet("/application-uptime", [HttpCacheIgnore] async (context) =>
        {
            TimeSpan elapsed = Stopwatch.Elapsed;
            await context.Response.WriteAsJsonAsync(elapsed, JsonSerializerOptions.Default, CancellationToken.None);
        }).AllowAnonymous();
    }

    private static void RegisterLifetimeEventHandlers(WebApplication app)
    {
        app.Logger.LogInformation("Registering application lifetime event handlers.");

        IHostApplicationLifetime lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

        lifetime.ApplicationStarted.Register(() =>
        {
            app.Logger.LogInformation(
                "Application started. Total startup time: {ElapsedMilliseconds}ms",
                Stopwatch.ElapsedMilliseconds
            );
        });

        lifetime.ApplicationStopping.Register(() =>
        {
            // We've kept the stopwatch running, so we can report total continuous uptime for a given instance.
            // Since the application is shutting down, we need to ensure the stopwatch is stopped.
            Stopwatch.Stop();

            // We can additionally report this final diagnostic information.
            app.Logger.LogInformation("Application is stopping. Total uptime: {Uptime}.", Stopwatch.Elapsed);
        });

        lifetime.ApplicationStopped.Register(() =>
        {
            app.Logger.LogInformation("Application has stopped.");
            Events.ClearHandlers();
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
