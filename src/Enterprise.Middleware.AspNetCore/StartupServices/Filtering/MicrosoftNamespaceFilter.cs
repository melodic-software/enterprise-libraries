using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Filtering;

public static class MicrosoftNamespaceFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue("excludeMicrosoft", out StringValues excludeMicrosoftFilterValue) ||
            !bool.TryParse(excludeMicrosoftFilterValue, out bool excludeMicrosoft))
        {
            return serviceDescriptors;
        }

        serviceDescriptors = excludeMicrosoft ?
            serviceDescriptors.Where(sd => !IsMicrosoftNamespace(sd.ServiceType.Namespace)).ToList() :
            serviceDescriptors.Where(sd => IsMicrosoftNamespace(sd.ServiceType.Namespace)).ToList();

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
