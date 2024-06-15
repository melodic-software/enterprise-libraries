using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Mapping;

public static class DtoCreationService
{
    public static ServiceDescriptionDto? CreateDto(ServiceDescriptor serviceDescriptor, HttpContext httpContext, ILogger logger)
    {
        try
        {
            string? instanceTypeName = serviceDescriptor.IsKeyedService ?
                GetKeyedInstanceTypeName(serviceDescriptor, httpContext.RequestServices) :
                GetInstanceTypeName(serviceDescriptor, httpContext.RequestServices);

            string? serviceKey = GetServiceKey(serviceDescriptor);

            return new ServiceDescriptionDto
            {
                ServiceTypeFullName = serviceDescriptor.ServiceType.FullName,
                ServiceLifetime = serviceDescriptor.Lifetime.ToString(),
                InstanceTypeName = instanceTypeName,
                ServiceKey = serviceKey
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the service description DTO.");
        }

        return null;
    }

    private static string? GetInstanceTypeName(ServiceDescriptor serviceDescriptor, IServiceProvider serviceProvider)
    {
        if (serviceDescriptor.ImplementationInstance != null)
        {
            // Singleton instances are provided directly.
            return serviceDescriptor.ImplementationInstance.GetType().FullName;
        }

        if (serviceDescriptor.ImplementationFactory != null)
        {
            // Attempt to resolve the service from the provider, respecting the service lifetime.
            object instance = serviceProvider.GetRequiredService(serviceDescriptor.ServiceType);
            return instance.GetType().FullName;
        }

        return null;
    }

    private static string? GetKeyedInstanceTypeName(ServiceDescriptor serviceDescriptor, IServiceProvider serviceProvider)
    {
        if (serviceDescriptor.KeyedImplementationInstance != null)
        {
            // Singleton instances provided directly.
            return serviceDescriptor.KeyedImplementationInstance.GetType().FullName;
        }

        if (serviceDescriptor.KeyedImplementationFactory != null)
        {
            // Attempt to resolve the service from the provider, respecting the service lifetime.
            object instance = serviceProvider.GetRequiredKeyedService(serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey);
            return instance.GetType().FullName;
        }

        return null;
    }

    private static string? GetServiceKey(ServiceDescriptor serviceDescriptor)
    {
        string? serviceKey = null;

        if (serviceDescriptor.ServiceKey?.GetType() == typeof(string))
        {
            serviceKey = serviceDescriptor.ServiceKey.ToString();
        }

        return serviceKey;
    }
}
