using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Mapping;

public static class DtoCreationService
{
    public static ServiceDescriptionDto? CreateDto(ServiceDescriptor serviceDescriptor, HttpContext httpContext, ILogger logger)
    {
        try
        {
            var dto = new ServiceDescriptionDto
            {
                ServiceTypeNamespace = serviceDescriptor.ServiceType.Namespace,
                ServiceTypeFullName = serviceDescriptor.ServiceType.FullName,
                ServiceTypeName = serviceDescriptor.ServiceType.Name,
                ServiceLifetime = serviceDescriptor.Lifetime.ToString()
            };

            if (serviceDescriptor.IsKeyedService)
            {
                dto.ServiceKey = GetServiceKey(serviceDescriptor);
                SetKeyedImplementationTypeProperties(serviceDescriptor, httpContext.RequestServices, dto);
            }
            else
            {
                SetImplementationTypeProperties(serviceDescriptor, httpContext.RequestServices, dto);
            }

            return dto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the service description DTO.");
        }

        return null;
    }

    private static void SetImplementationTypeProperties(ServiceDescriptor serviceDescriptor, IServiceProvider serviceProvider, ServiceDescriptionDto dto)
    {
        Type? implementationType = null;

        if (serviceDescriptor.ImplementationType != null)
        {
            implementationType = serviceDescriptor.ImplementationType;
        }
        else if (serviceDescriptor.ImplementationInstance != null)
        {
            // Singleton instances are provided directly.
            implementationType = serviceDescriptor.ImplementationInstance.GetType();
        }
        else if (serviceDescriptor.ImplementationFactory != null)
        {
            // Attempt to resolve the service from the provider, respecting the service lifetime.
            object instance = serviceProvider.GetRequiredService(serviceDescriptor.ServiceType);
            implementationType = instance.GetType();
        }

        dto.ImplementationTypeNamespace = implementationType?.Namespace;
        dto.ImplementationTypeFullName = implementationType?.FullName;
        dto.ImplementationTypeName = implementationType?.Name;
    }

    private static void SetKeyedImplementationTypeProperties(ServiceDescriptor serviceDescriptor, IServiceProvider serviceProvider, ServiceDescriptionDto dto)
    {
        Type? implementationType = null;

        if (serviceDescriptor.KeyedImplementationType != null)
        {
            implementationType = serviceDescriptor.KeyedImplementationType;
        }
        else if (serviceDescriptor.KeyedImplementationInstance != null)
        {
            // Singleton instances are provided directly.
            implementationType = serviceDescriptor.KeyedImplementationInstance.GetType();
        }
        else if (serviceDescriptor.KeyedImplementationFactory != null)
        {
            // Attempt to resolve the service from the provider, respecting the service lifetime.
            object instance = serviceProvider.GetRequiredKeyedService(serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey);
            implementationType = instance.GetType();
        }

        dto.ImplementationTypeNamespace = implementationType?.Namespace;
        dto.ImplementationTypeFullName = implementationType?.FullName;
        dto.ImplementationTypeName = implementationType?.Name;
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
