using System.Reflection;
using Enterprise.Logging.Core.Loggers;
using Enterprise.MediatR.Behaviors;
using Enterprise.MediatR.Options;
using Enterprise.Options.Core.Singleton;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.MediatR.Config;

public static class MediatRConfigService
{
    public static void ConfigureMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        MediatRConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatRConfigOptions>(configuration, MediatRConfigOptions.ConfigSectionKey);

        if (!options.EnableMediatR)
            return;

        Action<MediatRServiceConfiguration> configure = options.CustomConfigure ?? DefaultConfigure(options);

        services.AddMediatR(configure);
    }

    private static Action<MediatRServiceConfiguration> DefaultConfigure(MediatRConfigOptions options)
    {
        return mediatRConfiguration =>
        {
            Assembly[] assemblies = options.Assemblies.ToArray();
            bool explicitAssembliesSpecified = assemblies.Any();

            if (!explicitAssembliesSpecified)
            {
                // TODO: This fallback isn't ideal, as it could load a lot of assemblies we don't need.
                // We should try to find a more performant option to fall back to.
                PreStartupLogger.Instance.LogInformation("Explicit assemblies containing MediatR handlers have not been specified. Loading solution assemblies.");
                assemblies = AssemblyLoader.LoadSolutionAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);
            }

            List<BehaviorRegistration> behaviorRegistrations = options.BehaviorRegistrations;

            if (!behaviorRegistrations.Any())
                behaviorRegistrations = options.DefaultBehaviorRegistrations;

            if (explicitAssembliesSpecified)
            {
                PreStartupLogger.Instance.LogInformation("Registering MediatR handlers for the explicitly defined assemblies.");

                foreach (Assembly assembly in assemblies)
                {
                    PreStartupLogger.Instance.LogInformation(assembly.FullName);
                }
            }

            mediatRConfiguration.RegisterServicesFromAssemblies(assemblies);

            foreach (BehaviorRegistration behaviorRegistration in behaviorRegistrations)
            {
                mediatRConfiguration.AddOpenBehavior(behaviorRegistration.Type, behaviorRegistration.ServiceLifetime);
            }
        };
    }
}
