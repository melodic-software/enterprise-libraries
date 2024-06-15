using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ServiceKeyFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        // Filter by service key if specified.
        if (!query.TryGetValue(QueryStringConstants.ServiceKeys, out StringValues serviceKeysFilterValue))
        {
            return serviceDescriptors;
        }

        string[] serviceKeys = serviceKeysFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        serviceDescriptors = serviceDescriptors
            .Where(sd =>
                sd.ServiceKey is string &&
                serviceKeys.Contains(sd.ServiceKey.ToString(), StringComparer.OrdinalIgnoreCase))
            .ToList();

        return serviceDescriptors;
    }
}
