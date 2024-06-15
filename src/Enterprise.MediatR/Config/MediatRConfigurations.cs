using System.Reflection;
using Enterprise.MediatR.Assemblies;
using Enterprise.MediatR.Options;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.MediatR.Assemblies.AssemblyFallbackService;
using static Enterprise.MediatR.Assemblies.ExplicitAssemblyLogger;
using static Enterprise.MediatR.Behaviors.BehaviorRegistrar;

namespace Enterprise.MediatR.Config;
public static class MediatRConfigurations
{
    public static Action<MediatRServiceConfiguration> DefaultConfigure(MediatROptions options)
    {
        return mediatRServiceConfiguration =>
        {
            List<Assembly> assemblies = MediatRAssemblyService.Instance.AssembliesToRegister;

            bool explicitAssembliesSpecified = assemblies.Any();

            if (explicitAssembliesSpecified)
            {
                LogExplicitAssemblies(assemblies);
            }
            else
            {
                assemblies = GetAssemblies();
            }

            mediatRServiceConfiguration.RegisterServicesFromAssemblies([..assemblies]);

            RegisterBehaviors(options, mediatRServiceConfiguration);
        };
    }
}
