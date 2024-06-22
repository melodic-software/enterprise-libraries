using Enterprise.DI.Registration.Context.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Context;

public partial class RegistrationContext<TService> where TService : class
{
    public RegistrationContext<TService> RegisterAlternate<TAlternate>() where TAlternate : TService
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor originalDescriptor = ServiceDescriptorService.GetDescriptor(_services, serviceType);

        object ImplementationFactory(IServiceProvider serviceProvider)
        {
            TService originalService = ImplementationService.GetService<TService>(originalDescriptor, serviceProvider);
            return (TAlternate)originalService;
        }

        var alternateDescriptor = ServiceDescriptor.Describe(typeof(TAlternate), ImplementationFactory, originalDescriptor.Lifetime);

        _services.Add(alternateDescriptor);

        return this;
    }

    public RegistrationContext<TService> RegisterKeyedAlternate<TAlternate>(object? serviceKey) where TAlternate : TService
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor originalDescriptor = ServiceDescriptorService.GetDescriptor(_services, serviceType, serviceKey);

        object ImplementationFactory(IServiceProvider serviceProvider, object? _)
        {
            TService originalService = ImplementationService.GetService<TService>(originalDescriptor, serviceProvider);
            return (TAlternate)originalService;
        }

        var alternateDescriptor = ServiceDescriptor.DescribeKeyed(typeof(TAlternate), serviceKey, ImplementationFactory, originalDescriptor.Lifetime);

        _services.Add(alternateDescriptor);

        return this;
    }
}
