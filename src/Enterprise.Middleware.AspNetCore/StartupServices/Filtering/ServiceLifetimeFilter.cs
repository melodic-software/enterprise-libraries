using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Filtering;

public static class ServiceLifetimeFilter
{
    public static List<ServiceDescriptor> Apply(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        // Filter by service lifetime if specified.
        if (query.TryGetValue("lifetimes", out StringValues lifetimesFilterValue))
        {
            string[] lifetimes = lifetimesFilterValue.ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            serviceDescriptors = serviceDescriptors
                .Where(sd => lifetimes.Contains(sd.Lifetime.ToString(), StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        return serviceDescriptors;
    }
}
