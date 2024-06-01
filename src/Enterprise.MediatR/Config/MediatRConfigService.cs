using System.Reflection;
using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.MediatR.Assemblies.AssemblyRegistrar;
using static Enterprise.MediatR.Behaviors.BehaviorRegistrar;

namespace Enterprise.MediatR.Config;

public static class MediatRConfigService
{
    public static void ConfigureMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        MediatRConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatRConfigOptions>(configuration, MediatRConfigOptions.ConfigSectionKey);

        if (!options.EnableMediatR)
        {
            return;
        }

        Action<MediatRServiceConfiguration> configure = options.CustomConfigure ?? DefaultConfigure(options);

        services.AddMediatR(configure);
    }

    private static Action<MediatRServiceConfiguration> DefaultConfigure(MediatRConfigOptions options)
    {
        return mediatRConfiguration =>
        {
            Assembly[] assemblies = options.Assemblies.ToArray();
            bool explicitAssembliesSpecified = assemblies.Any();

            assemblies = explicitAssembliesSpecified ? 
                RegisterExplicitAssemblies(assemblies) :
                RegisterAssemblies();

            mediatRConfiguration.RegisterServicesFromAssemblies(assemblies);

            RegisterBehaviors(options, mediatRConfiguration);
        };
    }
}
