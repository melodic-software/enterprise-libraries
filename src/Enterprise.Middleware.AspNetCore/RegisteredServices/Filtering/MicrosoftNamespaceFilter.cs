using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class MicrosoftNamespaceFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue(QueryStringConstants.ExcludeMicrosoft, out StringValues excludeMicrosoftFilterValue) ||
            !bool.TryParse(excludeMicrosoftFilterValue, out bool excludeMicrosoft) || !excludeMicrosoft)
        {
            return serviceDescriptors;
        }

        serviceDescriptors = serviceDescriptors
            .Where(sd => !IsMicrosoftNamespace(sd.ServiceType.Namespace))
            .ToList();

        return serviceDescriptors;
    }

    private static bool IsMicrosoftNamespace(string? namespaceName)
    {
        return namespaceName != null &&
               (
                   namespaceName.StartsWith("System", StringComparison.OrdinalIgnoreCase) ||
                   namespaceName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase)
               );
    }
}
