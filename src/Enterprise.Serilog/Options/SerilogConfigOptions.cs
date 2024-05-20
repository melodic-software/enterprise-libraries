using Enterprise.Serilog.Templating;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Enterprise.Serilog.Options;

public class SerilogConfigOptions
{
    public const string ConfigSectionKey = "Custom:Serilog";

    /// <summary>
    /// Clear out all existing providers that have been registered before configuring Serilog.
    /// </summary>
    public bool ClearExistingProviders { get; set; }

    /// <summary>
    /// Set the default minimum level.
    /// NOTE: This will be ignored if a value has been provided in the application settings.
    /// </summary>
    public LogEventLevel DefaultMinimumLogLevel { get; set; } = LogEventLevel.Information;

    /// <summary>
    /// Configure the output template using the provided builder.
    /// </summary>
    public Action<IHostApplicationBuilder, OutputTemplateBuilder>? ConfigureOutputTemplate { get; set; }

    /// <summary>
    /// Configure the Serilog enrichers.
    /// Typically, this is something that is better to do in the application settings rather than in code configuration.
    /// NOTE: This will be ignored if a value has been provided in the application settings.
    /// </summary>
    public Action<LoggerConfiguration>? Enrich { get; set; }

    /// <summary>
    /// Configure the Serilog sinks.
    /// Typically, this is something that is better to do in the application settings rather than in code configuration.
    /// NOTE: This will be ignored if a value has been provided in the application settings.
    /// </summary>
    public Action<IHostApplicationBuilder, LoggerConfiguration, string>? WriteTo { get; set; }

    /// <summary>
    /// Allows for configuring how objects are destructured and transformed.
    /// One common example is to restrict what is used in logging statements like:
    /// logger.Information("New payment with data: {@PaymentData}", payment);
    /// </summary>
    public Action<LoggerDestructuringConfiguration>? ConfigureDestructuring { get; set; }

    /// <summary>
    /// Use this to completely override and control all Serilog logger configuration.
    /// NOTE: None of the defaults will be provided. This is a complete customization.
    /// </summary>
    public Action<HostBuilderContext, LoggerConfiguration>? CustomConfigureLogger { get; set; }

}
