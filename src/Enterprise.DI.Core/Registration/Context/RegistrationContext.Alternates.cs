using Enterprise.DI.Core.Registration.Context.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.DI.Core.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    public RegistrationContext<TService> RegisterAlternate<TAlternate>() where TAlternate : TService
    {
        Type serviceType = typeof(TService);

        ServiceDescriptor? originalServiceDescriptor = _services
            .FirstOrDefault(d => d.ServiceType == serviceType);

        if (originalServiceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {serviceType.Name} has not been registered and cannot be decorated."
            );
        }

        ServiceLifetime lifetime = originalServiceDescriptor.Lifetime;

        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService = ImplementationService.Get<TService>(originalServiceDescriptor, serviceProvider);
            return (TAlternate)originalService;
        }

        var alternateDescriptor = ServiceDescriptor.Describe(typeof(TAlternate), ImplementationFactory, lifetime);

        _services.TryAdd(alternateDescriptor);

        return this;
    }
}
