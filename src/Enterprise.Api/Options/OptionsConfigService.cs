using Enterprise.Applications.DI.Registration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Options;

internal static class OptionsConfigService
{
    internal static void ConfigureOptions(this IServiceCollection services, WebApplicationBuilder builder)
    {
        DefaultOptionsService.RegisterDefaults();
        SharedOptionsService.ConfigureShared(services, builder.Configuration);
        services.RegisterOptions(builder.Configuration);
    }
}
