using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Enterprise.MediatR.Config;

public static class MediatRConfigService
{
    public static void ConfigureMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        MediatROptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatROptions>(configuration, MediatROptions.ConfigSectionKey);

        if (!options.EnableMediatR)
        {
            return;
        }

        Action<MediatRServiceConfiguration> configure = 
            options.CustomConfigure ?? MediatRConfigurations.DefaultConfigure(options);

        services.AddMediatR(configure);
    }
}
