using Destructurama;
using Enterprise.Options.Core.Singleton;
using Enterprise.Serilog.AspNetCore.Lifecycle;
using Enterprise.Serilog.AspNetCore.RequestCorrelation;
using Enterprise.Serilog.MediatR;
using Enterprise.Serilog.Options;
using Enterprise.Serilog.Templating;
using Microsoft.AspNetCore.Builder;
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
        SerilogConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<SerilogConfigOptions>(builder.Configuration, SerilogConfigOptions.ConfigSectionKey);

        builder.ConfigureSerilog(configOptions);
    }

    public static void ConfigureSerilog(this WebApplicationBuilder builder, SerilogConfigOptions configOptions)
    {
        // Example service that could manage lifecycle.
        builder.Services.AddHostedService<LifecycleHostedService>();

        if (configOptions.ClearExistingProviders)
            builder.Logging.ClearProviders();

        Action<HostBuilderContext, LoggerConfiguration> configureLogger =
            configOptions.CustomConfigureLogger ?? ConfigureLogger(builder, configOptions);

        // This essentially "hijacks" the .NET logging infrastructure to use Serilog.
        // Setting "preserveStaticLogger" to false replaces Log.Logger with the logger instance configured in the provided delegate.
        // Setting "writeToProviders" to false prevents Serilog from writing to other logging providers configured in the ASP.NET Core logging system.
        builder.Host.UseSerilog(
            configureLogger,
            preserveStaticLogger: false,
            writeToProviders: false
        );

        builder.RegisterMediatRServices();
    }

    public static void UseSerilog(this WebApplication app)
    {
        app.UseMiddleware<RequestCorrelationMiddleware>();

        // This introduces middleware that hooks into incoming API requests
        // and starts logging useful information about the processing of API requests
        // such as status codes, request times, any exceptions, and so on.
        app.UseSerilogRequestLogging();
    }

    private static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger(IHostApplicationBuilder builder, SerilogConfigOptions configOptions)
    {
        configOptions.ConfigureOutputTemplate ??= SerilogConfigDefaults.CreateDefaultOutputTemplate;
        configOptions.Enrich ??= SerilogConfigDefaults.EnrichDefaults;
        configOptions.WriteTo ??= SerilogConfigDefaults.WriteToDefaults;

        return (context, loggerConfig) =>
        {
            loggerConfig.MinimumLevel.Information();

            OutputTemplateBuilder outputTemplateBuilder = new OutputTemplateBuilder();
            configOptions.ConfigureOutputTemplate(builder, outputTemplateBuilder);
            string outputTemplate = outputTemplateBuilder.Build();

            configOptions.Enrich(loggerConfig);
            configOptions.WriteTo(builder, loggerConfig, outputTemplate);

            // This enables the use of attributes to apply masking configuration.
            // Projects will need to reference the Destructurama.Attributes package to apply the attributes.
            loggerConfig.Destructure.UsingAttributes();

            configOptions.ConfigureDestructuring?.Invoke(loggerConfig.Destructure);

            // Allow config overrides.
            loggerConfig.ReadFrom.Configuration(context.Configuration);

            SelfLog.Enable(Console.Error);
        };
    }
}