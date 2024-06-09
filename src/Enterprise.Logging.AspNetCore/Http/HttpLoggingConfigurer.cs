using Enterprise.Options.Core.Services.Singleton;
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
        HttpLoggingOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<HttpLoggingOptions>(configuration, HttpLoggingOptions.ConfigSectionKey);

        if (!options.EnableHttpLogging)
        {
            return;
        }

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging

        services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.All;

            // Request headers that are allowed to be logged.
            o.RequestHeaders.Add(SecChUa);

            o.MediaTypeOptions.AddText(Javascript);

            o.RequestBodyLogLimit = options.RequestBodyLogLimit;
            o.ResponseBodyLogLimit = options.ResponseBodyLogLimit;
        });
    }

    internal static void UseHttpLogging(this WebApplication app)
    {
        HttpLoggingOptions options = app.Services.GetRequiredService<IOptions<HttpLoggingOptions>>().Value;

        if (!options.EnableHttpLogging)
        {
            return;
        }

        app.UseHttpLogging();
    }
}
