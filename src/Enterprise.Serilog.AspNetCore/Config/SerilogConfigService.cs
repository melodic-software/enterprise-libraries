using Destructurama;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Serilog.AspNetCore.Lifecycle;
using Enterprise.Serilog.AspNetCore.RequestCorrelation;
using Enterprise.Serilog.Options;
using Enterprise.Serilog.Options.Delegates;
using Enterprise.Serilog.Templating;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;

namespace Enterprise.Serilog.AspNetCore.Config;

public static class SerilogConfigService
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        SerilogOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<SerilogOptions>(builder.Configuration, SerilogOptions.ConfigSectionKey);

        builder.ConfigureSerilog(options);
    }

    public static void ConfigureSerilog(this WebApplicationBuilder builder, SerilogOptions options)
    {
        // Example service that could manage lifecycle.
        builder.Services.AddHostedService<LifecycleHostedService>();

        if (options.ClearExistingProviders)
        {
            builder.Logging.ClearProviders();
        }

        ConfigureLogger configureLogger = options.ConfigureLogger ?? ConfigureLogger(builder, options);

        // This essentially "hijacks" the .NET logging infrastructure to use Serilog.
        // Setting "preserveStaticLogger" to false replaces Log.Logger with the logger instance configured in the provided delegate.
        // Setting "writeToProviders" to false prevents Serilog from writing to other logging providers configured in the ASP.NET Core logging system.
        builder.Host.UseSerilog(
            configureLogger.Invoke,
            preserveStaticLogger: false,
            writeToProviders: false
        );
    }

    public static void UseSerilog(this WebApplication app)
    {
        app.UseRequestCorrelationMiddleware();

        // This introduces middleware that hooks into incoming API requests
        // and starts logging useful information about the processing of API requests
        // such as status codes, request times, any exceptions, and so on.
        app.UseSerilogRequestLogging();
    }

    private static ConfigureLogger ConfigureLogger(IHostApplicationBuilder builder, SerilogOptions options)
    {
        options.ConfigureOutputTemplate ??= SerilogConfigDefaults.CreateDefaultOutputTemplate;
        options.Enrich ??= SerilogConfigDefaults.EnrichDefaults;

        options.WriteTo ??= (_, loggerConfig, outputTemplate) =>
            SerilogConfigDefaults.WriteToDefaults(loggerConfig, outputTemplate);

        return (context, loggerConfig) =>
        {
            // Apply minimal log level programmatically if not defined in configuration.
            if (!context.Configuration.GetSection("Serilog:MinimumLevel:Default").Exists())
            {
                loggerConfig.MinimumLevel.Is(options.DefaultMinimumLogLevel);
            }

            // Apply enrichments programmatically if not defined in configuration.
            if (!context.Configuration.GetSection("Serilog:Enrich").Exists())
            {
                options.Enrich(loggerConfig);
            }

            // Apply sinks programmatically if not defined in configuration.
            if (!context.Configuration.GetSection("Serilog:WriteTo").Exists())
            {
                var outputTemplateBuilder = new OutputTemplateBuilder();
                options.ConfigureOutputTemplate(builder, outputTemplateBuilder);
                string outputTemplate = outputTemplateBuilder.Build();

                options.WriteTo(builder, loggerConfig, outputTemplate);
            }

            // This enables the use of attributes to apply masking configuration.
            // Projects will need to reference the Destructurama.Attributes package to apply the attributes.
            loggerConfig.Destructure.UsingAttributes();
            options.ConfigureDestructuring?.Invoke(loggerConfig.Destructure);

            // This will allow for specifying values in app settings
            // The default behavior merges these values with the programmatic configuration, which can result in undesired behavior (particularly with sinks).
            // We've added checks above to specifically support one or the other.
            loggerConfig.ReadFrom.Configuration(context.Configuration);

            SelfLog.Enable(Console.Error);
        };
    }
}
