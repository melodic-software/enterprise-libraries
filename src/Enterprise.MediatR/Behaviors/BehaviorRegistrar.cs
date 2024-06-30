using Enterprise.MediatR.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Behaviors;

internal static class BehaviorRegistrar
{
    public static void RegisterBehaviors(MediatROptions options, MediatRServiceConfiguration mediatRServiceConfiguration)
    {
        IReadOnlyCollection<BehaviorRegistration> behaviorRegistrations = options.BehaviorRegistrations;

        foreach (BehaviorRegistration behaviorRegistration in behaviorRegistrations)
        {
            mediatRServiceConfiguration.AddOpenBehavior(behaviorRegistration.Type, behaviorRegistration.ServiceLifetime);
        }
    }
}
