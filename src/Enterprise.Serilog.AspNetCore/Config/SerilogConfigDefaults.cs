using System.Reflection;
using Enterprise.Serilog.Templating;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Enterprise.Serilog.AspNetCore.Config;

public static class SerilogConfigDefaults
{
    public const string Application = "Application";
    public const string InvalidAssemblyReferenceExceptionMessage = "Invalid assembly reference!";
    public const string InvalidAssemblyNameExceptionMessage = "The assembly name is invalid!";
    public const string SourceContextTemplate = "{SourceContext}";

    public static void CreateDefaultOutputTemplate(IHostApplicationBuilder builder, OutputTemplateBuilder outputTemplateBuilder)
    {
        if (builder.Environment.IsDevelopment())
        {
            outputTemplateBuilder
                .SetCustomEnrichmentTemplate(SourceContextTemplate)
                .UseSimpleTimeFormat();
        }
    }

    public static void EnrichDefaults(LoggerConfiguration loggerConfig)
    {
        Assembly assembly = Assembly.GetEntryAssembly() ?? throw new Exception(InvalidAssemblyReferenceExceptionMessage);

        string? assemblyName = assembly.GetName().Name;

        if (string.IsNullOrWhiteSpace(assemblyName))
            throw new Exception(InvalidAssemblyNameExceptionMessage);

        loggerConfig
            .Enrich.WithProperty(Application, assemblyName)
            .Enrich.FromLogContext();
    }

    public static void WriteToDefaults(IHostApplicationBuilder builder, LoggerConfiguration loggerConfig, string outputTemplate)
    {
        loggerConfig
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: outputTemplate,
                theme: AnsiConsoleTheme.Code
            )
            .WriteTo.Debug();
    }
}