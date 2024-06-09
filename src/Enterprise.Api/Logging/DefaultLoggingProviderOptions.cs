using Enterprise.Logging.Providers;
using Enterprise.Serilog.AspNetCore.Config;
using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Logging;

internal sealed class DefaultLoggingProviderOptions : LoggingProviderOptions
{
    internal DefaultLoggingProviderOptions()
    {
        ConfigureProviders = builder =>
        {
            if (builder is not WebApplicationBuilder applicationBuilder)
            {
                return;
            }

            applicationBuilder.ConfigureSerilog();
        };
    }
}
