using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration.Context.Services;

internal static class ImplementationService
{
    internal static TService Get<TService>(ServiceDescriptor serviceDescriptor, IServiceProvider provider) where TService : class
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

        throw new InvalidOperationException("The registration method for the service is not supported.");
    }
}
