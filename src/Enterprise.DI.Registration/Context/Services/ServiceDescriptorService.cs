using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Context.Services;

internal static class ServiceDescriptorService
{
    internal static ServiceDescriptor GetDescriptor(IServiceCollection services, Type serviceType, object? serviceKey = null)
    {
        ServiceDescriptor? serviceDescriptor = services
            .LastOrDefault(d =>
                d.ServiceType == serviceType &&
                (serviceKey is null || d.IsKeyedService && d.ServiceKey == serviceKey)
            );

        if (serviceDescriptor == null)
        {
            throw new InvalidOperationException(
                $"The service of type {serviceType.Name} has not been registered."
            );
        }

        return serviceDescriptor;
    }
}
