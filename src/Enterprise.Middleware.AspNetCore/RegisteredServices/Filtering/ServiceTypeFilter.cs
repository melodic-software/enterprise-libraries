using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ServiceTypeFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue(QueryStringConstants.ServiceTypes, out StringValues serviceTypeNamesFilterValue))
        {
            return serviceDescriptors;
        }

        string[] serviceTypes = serviceTypeNamesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        serviceDescriptors = serviceDescriptors
            .Where(sd =>
                sd.ServiceType.FullName != null &&
                serviceTypes.Any(stn => sd.ServiceType.FullName.StartsWith(stn, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return serviceDescriptors;
    }
}
