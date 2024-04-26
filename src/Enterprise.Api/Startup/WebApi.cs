using Enterprise.Api.Diagnostics;
using Enterprise.Api.Options;
using Enterprise.Logging.Core.Loggers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Enterprise.Api.Startup;

/// <summary>
/// Configure services, request pipeline middleware, and startup the API.
/// </summary>
public class WebApi
{
    public static async Task RunAsync(string[] args) => await RunAsync(args, _ => { });

    /// <summary>
    /// Configures the application's services and request pipeline.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <param name="configure">A delegate to configure <see cref="ApiConfigOptions"/>.</param>
    public static async Task RunAsync(string[] args, Action<ApiConfigOptions>? configure)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        ApiConfigOptions options = new ApiConfigOptions(args);

        try
        {
            PreStartupLogger.Instance.LogInformation("Beginning configuration.");

            // This is the primary hook for applications to configure the API.
            // This must be placed first in case the application decides to wire up event handlers.
            configure?.Invoke(options);

            await options.Events.RaisePreConfigurationCompleted(args);

            WebApplicationBuilder builder = await CreateBuilderAsync(options);

            // Execute deferred configurations with the actual configuration and environment.
            PreStartupLogger.Instance.LogInformation("Executing deferred option configuration.");
            options.ExecuteDeferredConfigurations(builder.Configuration, builder.Environment);
            PreStartupLogger.Instance.LogInformation("Deferred option configuration complete.");

            await AddServicesAsync(builder, options);
            WebApplication app = await BuildApplicationAsync(builder, options);

            await ConfigureRequestPipelineAsync(builder, app, options);

            app.Logger.LogInformation("API configuration is complete.");
            await options.Events.RaiseConfigurationCompleted();

            RegisterLifetimeEventHandlers(app, options, stopwatch);

            await RunApplicationAsync(app);
        }
        catch (Exception ex)
        {
            PreStartupLogger.Instance.LogError(ex, "An error has occurred during API configuration.");
            await options.Events.RaiseConfigurationErrorOccurred(ex);
            throw;
        }
    }

    /// <summary>
    /// Create the WebApplicationBuilder.
    /// This calls into the default builder creation method which registers logging providers, and other service defaults.
    /// https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebHost.cs#L155
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private static async Task<WebApplicationBuilder> CreateBuilderAsync(ApiConfigOptions options)
    {
        PreStartupLogger.Instance.LogInformation("Creating the WebApplicationBuilder.");
        WebApplicationBuilder builder = WebApplication.CreateBuilder(options.WebApplicationOptions);
        await options.Events.RaiseBuilderCreated(builder);
        PreStartupLogger.Instance.LogInformation("WebApplicationBuilder created.");
        return builder;
    }

    /// <summary>
    /// Add services to the container.
    /// This includes framework services, and application specific services.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    private static async Task AddServicesAsync(WebApplicationBuilder builder, ApiConfigOptions options)
    {
        PreStartupLogger.Instance.LogInformation("Adding services to the DI container.");
        builder.ConfigureServices();
        PreStartupLogger.Instance.LogInformation("Services have been registered with the DI container.");
        await options.Events.RaiseServicesConfigured(builder);
    }

    /// <summary>
    /// Build the application.
    /// All services must be registered before this is called.
    /// Any attempts to add additional services will result in an exception.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private static async Task<WebApplication> BuildApplicationAsync(WebApplicationBuilder builder, ApiConfigOptions options)
    {
        PreStartupLogger.Instance.LogInformation("Building the application.");

        builder.InsertAnalysisStartupFilter();

        WebApplication app = builder.Build();

        PreStartupLogger.Instance.SetLogger(app.Logger);
        app.Logger.LogInformation("Application has been built.");

        await options.Events.RaiseWebApplicationBuilt(app);

        return app;
    }

    /// <summary>
    /// Configure the HTTP request (middleware) pipeline.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="app"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private static async Task ConfigureRequestPipelineAsync(IHostApplicationBuilder builder, WebApplication app, ApiConfigOptions options)
    {
        app.Logger.LogInformation("Configuring the request pipeline.");
        app.ConfigureRequestPipeline(builder);
        app.Logger.LogInformation("Request pipeline has been configured.");
        await options.Events.RaiseRequestPipelineConfigured(app);
    }

    private static void RegisterLifetimeEventHandlers(WebApplication app, ApiConfigOptions options, Stopwatch stopwatch)
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
            options.Events.ClearHandlers();
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
