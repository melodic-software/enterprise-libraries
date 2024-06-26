using Enterprise.Api.SignalR.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.SignalR.Config;

public static class SignalRConfigurer
{
    public static void ConfigureSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        SignalROptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<SignalROptions>(configuration, SignalROptions.ConfigSectionKey);

        if (!options.SignalREnabled)
        {
            return;
        }

        services.AddSignalR(hubOptions =>
        {
            options.ConfigureHubOptions?.Invoke(hubOptions);
        });
    }

    public static void UseSignalR(this WebApplication app)
    {
        SignalROptions options = app.Services.GetRequiredService<IOptions<SignalROptions>>().Value;

        if (!options.SignalREnabled)
        {
            return;
        }

        options.MapHubs?.Invoke(app);
    }
}
