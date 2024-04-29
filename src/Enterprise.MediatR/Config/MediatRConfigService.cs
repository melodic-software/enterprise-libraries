using Enterprise.MediatR.Options;
using Enterprise.Reflection.Assemblies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Enterprise.Options.Core.Singleton;
using Enterprise.MediatR.Behaviors.Logging;
using Enterprise.MediatR.Behaviors.Validation;
using Enterprise.MediatR.Behaviors.Caching;

namespace Enterprise.MediatR.Config;

public static class MediatRConfigService
{
    public static void ConfigureMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        MediatRConfigOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MediatRConfigOptions>(configuration, MediatRConfigOptions.ConfigSectionKey);

        if (!options.EnableMediatR)
            return;

        // TODO: This fallback isn't ideal, as it could load a lot of assemblies we don't need.
        // We should try to find a more performant option to fall back to.
        Func<Assembly[]> getAssemblies = options.GetServicesFromAssemblies ?? GetAssemblies;

        services.AddMediatR(mediatRConfiguration =>
        {
            Assembly[] assemblies = getAssemblies();

            mediatRConfiguration.RegisterServicesFromAssemblies(assemblies);

            mediatRConfiguration.AddOpenBehavior(typeof(GlobalRequestLoggingBehavior<,>), ServiceLifetime.Scoped);
            mediatRConfiguration.AddOpenBehavior(typeof(UseCaseLoggingBehavior<,>));
            mediatRConfiguration.AddOpenBehavior(typeof(CommandFluentValidationBehavior<,>));
            mediatRConfiguration.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });
    }

    private static Assembly[] GetAssemblies()
    {
        Assembly[] assemblies = AssemblyLoader.LoadSolutionAssemblies(AssemblyFilterPredicates.ThatAreNotMicrosoft);

        return assemblies;
    }
}
