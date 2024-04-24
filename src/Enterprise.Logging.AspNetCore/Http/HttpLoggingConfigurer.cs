using Enterprise.Options.Core.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Enterprise.Constants.MediaTypeConstants;
using static Enterprise.Constants.RequestHeaderConstants;

namespace Enterprise.Logging.AspNetCore.Http;

internal static class HttpLoggingConfigurer
{
    internal static void ConfigureHttpLogging(this IServiceCollection services, IConfiguration configuration)
    {
        HttpLoggingConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<HttpLoggingConfigOptions>(configuration, HttpLoggingConfigOptions.ConfigSectionKey);

        if (!configOptions.EnableHttpLogging)
            return;

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All;

            // Request headers that are allowed to be logged.
            options.RequestHeaders.Add(SecChUa);

            options.MediaTypeOptions.AddText(Javascript);

            options.RequestBodyLogLimit = configOptions.RequestBodyLogLimit;
            options.ResponseBodyLogLimit = configOptions.ResponseBodyLogLimit;
        });
    }

    internal static void UseHttpLogging(this WebApplication app)
    {
        HttpLoggingConfigOptions configOptions = app.Services.GetRequiredService<IOptions<HttpLoggingConfigOptions>>().Value;

        if (!configOptions.EnableHttpLogging)
            return;

        app.UseHttpLogging();
    }
}