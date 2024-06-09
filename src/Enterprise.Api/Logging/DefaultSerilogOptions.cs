using System.Globalization;
using System.Reflection;
using Enterprise.Serilog.Options;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Enterprise.Api.Logging;

public class DefaultSerilogOptions : SerilogOptions
{
    public DefaultSerilogOptions()
    {
        ConfigureOutputTemplate = (builder, outputTemplateBuilder) =>
        {
            if (builder.Environment.IsDevelopment())
            {
                outputTemplateBuilder
                    .SetCustomEnrichmentTemplate("{SourceContext}")
                    .UseSimpleTimeFormat();
            }
        };

        Enrich = loggerConfig =>
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? throw new Exception("Invalid assembly reference!");

            string? assemblyName = assembly.GetName().Name;

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new Exception("The assembly name is invalid!");
            }

            loggerConfig
                .Enrich.WithProperty("Application", assemblyName)
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .Enrich.With<ActivityEnricher>()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithAssemblyInformationalVersion()
                .Enrich.WithMemoryUsage()
                .Enrich.WithCorrelationId(headerName: "X-Correlation-Id", addValueIfHeaderAbsence: true)
                .Enrich.WithRequestHeader("User-Agent")
                .Enrich.WithRequestHeader("Connection")
                .Enrich.WithRequestHeader("Content-Length", "RequestLength");
        };

        WriteTo = (builder, loggerConfig, outputTemplate) =>
        {
            loggerConfig
                .WriteTo.Console(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: outputTemplate,
                    theme: AnsiConsoleTheme.Code,
                    formatProvider: CultureInfo.InvariantCulture
                )
                .WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture);
        };
    }
}
