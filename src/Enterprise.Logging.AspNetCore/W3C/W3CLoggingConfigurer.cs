using Enterprise.Options.Core.Singleton;
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
        W3CLoggingConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<W3CLoggingConfigOptions>(configuration, W3CLoggingConfigOptions.ConfigSectionKey);

        if (!configOptions.EnableW3CLogging)
            return;

        ArgumentException.ThrowIfNullOrWhiteSpace(configOptions.W3CLogFileApplicationName);

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string logDirectory = Path.Combine(appDataPath, LogDirectoryName);
        string fileNamePrefix = GetFileNamePrefix(configOptions.W3CLogFileApplicationName);

        services.AddW3CLogging(options =>
        {
            options.LoggingFields = W3CLoggingFields.All;
            options.AdditionalRequestHeaders.Add(XForwardedFor);
            options.AdditionalRequestHeaders.Add(XClientSslProtocol);
            options.FileSizeLimit = FileSizeLimitInBytes;
            options.RetainedFileCountLimit = 2; // TODO: verify if this needs to change
            options.FileName = fileNamePrefix; // This gets used as a prefix.
            options.LogDirectory = logDirectory;
            options.FlushInterval = TimeSpan.FromSeconds(FlushIntervalInSeconds);
        });
    }

    internal static void UseW3CLogging(this WebApplication app)
    {
        W3CLoggingConfigOptions configOptions = app.Services.GetRequiredService<IOptions<W3CLoggingConfigOptions>>().Value;

        if (!configOptions.EnableW3CLogging)
            return;

        HttpLoggingBuilderExtensions.UseW3CLogging(app);
    }
}