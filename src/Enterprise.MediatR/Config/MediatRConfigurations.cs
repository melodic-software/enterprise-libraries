using System.Reflection;
using Enterprise.MediatR.Options;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.MediatR.Assemblies.AssemblyRegistrar;
using static Enterprise.MediatR.Behaviors.BehaviorRegistrar;

namespace Enterprise.MediatR.Config;
public static class MediatRConfigurations
{
    public static Action<MediatRServiceConfiguration> DefaultConfigure(MediatROptions options)
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
