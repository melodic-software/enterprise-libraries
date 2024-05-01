using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Behaviors;

public class BehaviorRegistration
{
    public Type Type { get; }
    public ServiceLifetime ServiceLifetime { get; }

    public BehaviorRegistration(Type type, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        Type = type;
        ServiceLifetime = serviceLifetime;
    }
}
