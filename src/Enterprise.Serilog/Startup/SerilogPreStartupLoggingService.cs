﻿using System.Globalization;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Serilog.Templating;
using Serilog;

namespace Enterprise.Serilog.Startup;

public static class SerilogPreStartupLoggingService
{
    /// <summary>
    /// Replaces the pre-startup logger with a Serilog implementation until the Microsoft ILogger instance is configured.
    /// </summary>
    public static void ConfigurePreStartupLogger()
    {
        var outputTemplateBuilder = new OutputTemplateBuilder();
        outputTemplateBuilder.UseSimpleTimeFormat();
        string defaultOutputTemplate = outputTemplateBuilder.Build();

        // This is a temporary logger that can be used on application startup.
        ILogger logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: defaultOutputTemplate,
                formatProvider: CultureInfo.InvariantCulture
            )
            .CreateLogger();

        // Set the global Serilog logger.
        // This will likely be replaced in the official logger configuration.
        // It requires Serilog to be registered as a logging provider for Microsoft's own ILogger implementation.
        Log.Logger = logger;

        // At this point we can wrap the global serilog logger and use it for the all the pre-startup logging.
        // This instance will get replaced with the official Microsoft ILogger when the service provider is built.
        var preStartupSerilogWrapper = new PreStartupSerilogWrapper();
        PreStartupLogger.Instance.SetLogger(preStartupSerilogWrapper);
    }
}
