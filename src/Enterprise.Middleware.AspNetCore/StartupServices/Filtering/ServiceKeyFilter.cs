using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Filtering;

public static class ServiceKeyFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        // Filter by service key if specified.
        if (!query.TryGetValue("serviceKeys", out StringValues serviceKeysFilterValue))
        {
            return serviceDescriptors;
        }

        string[] serviceKeys = serviceKeysFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        serviceDescriptors = serviceDescriptors
            .Where(sd =>
                sd.ServiceKey is string &&
                serviceKeys.Contains(sd.ServiceKey.ToString(), StringComparer.OrdinalIgnoreCase))
            .ToList();

        return serviceDescriptors;
    }
}
