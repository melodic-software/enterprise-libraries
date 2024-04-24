using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Controllers.Formatters;

public static class FormatterPrinter
{
    public static void PrintInputFormatters(IMvcBuilder builder)
    {
        // Print out registered input formatters after configuration is complete.
        builder.Services.PostConfigure<MvcOptions>(options =>
        {
            foreach (IInputFormatter formatter in options.InputFormatters)
            {
                // Check if the formatter is of a type that has the SupportedMediaTypes property.
                if (formatter is InputFormatter inputFormatter)
                {
                    MediaTypeCollection mediaTypes = inputFormatter.SupportedMediaTypes;

                    string message = $"Registered Input Formatter: {formatter.GetType().Name}, " +
                                     $"Supported Media Types: {string.Join(", ", mediaTypes)}";

                    PreStartupLogger.Instance.LogDebug(message);
                }
                else
                {
                    // For formatters not derived from InputFormatter, just print the type name
                    PreStartupLogger.Instance.LogDebug($"Registered Input Formatter: {formatter.GetType().Name}");
                }
            }
        });
    }

    public static void PrintOutputFormatters(IMvcBuilder builder)
    {
        // Print out registered output formatters after configuration is complete.
        builder.Services.PostConfigure<MvcOptions>(options =>
        {
            foreach (IOutputFormatter formatter in options.OutputFormatters)
            {
                // Check if the formatter is of a type that has the SupportedMediaTypes property.
                if (formatter is OutputFormatter outputFormatter)
                {
                    MediaTypeCollection mediaTypes = outputFormatter.SupportedMediaTypes;

                    string message = $"Registered Output Formatter: {formatter.GetType().Name}, " +
                                     $"Supported Media Types: {string.Join(", ", mediaTypes)}";

                    PreStartupLogger.Instance.LogDebug(message);
                }
                else
                {
                    // For formatters not derived from OutputFormatter, just print the type name.
                    PreStartupLogger.Instance.LogDebug($"Registered Output Formatter: {formatter.GetType().Name}");
                }
            }
        });
    }
}