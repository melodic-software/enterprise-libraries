using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration.Services;

public static class ImplementationService
{
    public static TService GetImplementation<TService>(ServiceDescriptor serviceDescriptor, IServiceProvider provider) where TService : class
    {
        // TODO: Are we concerned at all about InvalidCastException if the returned object is not of TService?

        if (serviceDescriptor.IsKeyedService)
        {
            if (serviceDescriptor.KeyedImplementationFactory != null)
            {
                return (TService)serviceDescriptor.KeyedImplementationFactory(provider, serviceDescriptor.ServiceKey);
            }

            if (serviceDescriptor.KeyedImplementationInstance != null)
            {
                return (TService)serviceDescriptor.KeyedImplementationInstance;
            }

            if (serviceDescriptor.KeyedImplementationType != null)
            {
                return (TService)ActivatorUtilities.CreateInstance(provider, serviceDescriptor.KeyedImplementationType);
            }
        }
        else
        {
            if (serviceDescriptor.ImplementationFactory != null)
            {
                return (TService)serviceDescriptor.ImplementationFactory(provider);
            }

            if (serviceDescriptor.ImplementationInstance != null)
            {
                return (TService)serviceDescriptor.ImplementationInstance;
            }

            if (serviceDescriptor.ImplementationType != null)
            {
                return (TService)ActivatorUtilities.CreateInstance(provider, serviceDescriptor.ImplementationType);
            }
        }

        throw new InvalidOperationException("The registration method for the service is not supported.");
    }
}
