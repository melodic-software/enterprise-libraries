using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ServiceLifetimeFilter
{
    public static List<ServiceDescriptor> Apply(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue(QueryStringConstants.ServiceLifetimes, out StringValues serviceLifetimesFilterValue))
        {
            return serviceDescriptors;
        }

        string[] serviceLifetimes = serviceLifetimesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        serviceDescriptors = serviceDescriptors
            .Where(sd => serviceLifetimes.Contains(sd.Lifetime.ToString(), StringComparer.OrdinalIgnoreCase))
            .ToList();

        return serviceDescriptors;
    }
}
