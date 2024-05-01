using Enterprise.MediatR.Options;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Enterprise.MediatR.Behaviors;
using Enterprise.Options.Core.Singleton;

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

    private static Assembly[] GetAssemblies()
    {
        Assembly[] assemblies = AssemblyLoader.LoadSolutionAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);

        return assemblies;
    }

    private static Action<MediatRServiceConfiguration> DefaultConfigure(MediatRConfigOptions options)
    {
        // TODO: This fallback isn't ideal, as it could load a lot of assemblies we don't need.
        // We should try to find a more performant option to fall back to.
        Func<Assembly[]> getAssemblies = options.GetAssemblies ?? GetAssemblies;
        List<BehaviorRegistration> behaviorRegistrations = options.BehaviorRegistrations;

        if (!behaviorRegistrations.Any())
            behaviorRegistrations = options.DefaultBehaviorRegistrations;

        return mediatRConfiguration =>
        {
            Assembly[] assemblies = getAssemblies();

            mediatRConfiguration.RegisterServicesFromAssemblies(assemblies);

            foreach (BehaviorRegistration behaviorRegistration in behaviorRegistrations)
            {
                mediatRConfiguration.AddOpenBehavior(behaviorRegistration.Type, behaviorRegistration.ServiceLifetime);
            }
        };
    }
}
