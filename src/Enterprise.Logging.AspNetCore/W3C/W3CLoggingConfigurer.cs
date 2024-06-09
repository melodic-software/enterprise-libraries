using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Enterprise.Constants.RequestHeaderConstants;
using static Enterprise.Logging.AspNetCore.W3C.W3CLoggingConstants;

namespace Enterprise.Logging.AspNetCore.W3C;

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger

internal static class W3CLoggingConfigurer
{
    internal static void ConfigureW3CLogging(this IServiceCollection services, IConfiguration configuration)
    {
        W3CLoggingOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<W3CLoggingOptions>(configuration, W3CLoggingOptions.ConfigSectionKey);

        if (!options.EnableW3CLogging)
        {
            return;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(options.W3CLogFileApplicationName);

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string logDirectory = Path.Combine(appDataPath, LogDirectoryName);
        string fileNamePrefix = GetFileNamePrefix(options.W3CLogFileApplicationName);

        services.AddW3CLogging(o =>
        {
            o.LoggingFields = W3CLoggingFields.All;
            o.AdditionalRequestHeaders.Add(XForwardedFor);
            o.AdditionalRequestHeaders.Add(XClientSslProtocol);
            o.FileSizeLimit = FileSizeLimitInBytes;
            o.RetainedFileCountLimit = 2; // TODO: verify if this needs to change
            o.FileName = fileNamePrefix; // This gets used as a prefix.
            o.LogDirectory = logDirectory;
            o.FlushInterval = TimeSpan.FromSeconds(FlushIntervalInSeconds);
        });
    }

    internal static void UseW3CLogging(this WebApplication app)
    {
        W3CLoggingOptions options = app.Services.GetRequiredService<IOptions<W3CLoggingOptions>>().Value;

        if (!options.EnableW3CLogging)
        {
            return;
        }

        HttpLoggingBuilderExtensions.UseW3CLogging(app);
    }
}
