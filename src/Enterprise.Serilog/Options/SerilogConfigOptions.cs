using Enterprise.Serilog.Templating;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;

namespace Enterprise.Serilog.Options;

public class SerilogConfigOptions
{
    public const string ConfigSectionKey = "Custom:Serilog";

    /// <summary>
    /// Clear out all existing providers that have been registered before configuring Serilog.
    /// </summary>
    public bool ClearExistingProviders { get; set; } = false;

    /// <summary>
    /// Configure the output template using the provided builder.
    /// </summary>
    public Action<IHostApplicationBuilder, OutputTemplateBuilder>? ConfigureOutputTemplate { get; set; } = null;

    /// <summary>
    /// Configure the Serilog enrichers.
    /// Typically, this is something that is better to do in the application settings JSON rather than in code configuration.
    /// </summary>
    public Action<LoggerConfiguration>? Enrich { get; set; } = null;

    /// <summary>
    /// Configure the Serilog sinks.
    /// Typically, this is something that is better to do in the application settings JSON rather than in code configuration.
    /// </summary>
    public Action<IHostApplicationBuilder, LoggerConfiguration, string>? WriteTo { get; set; } = null;

    /// <summary>
    /// Allows for configuring how objects are destructured and transformed.
    /// One common example is to restrict what is used in logging statements like:
    /// logger.Information("New payment with data: {@PaymentData}", payment);
    /// </summary>
    public Action<LoggerDestructuringConfiguration>? ConfigureDestructuring { get; set; } = null;

    /// <summary>
    /// Use this to completely override and control all Serilog logger configuration.
    /// </summary>
    public Action<HostBuilderContext, LoggerConfiguration>? CustomConfigureLogger { get; set; } = null;

}