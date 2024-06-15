using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Filtering;

public static class ServiceTypeFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue("serviceTypeNames", out StringValues serviceTypeNamesFilterValue))
        {
            return serviceDescriptors;
        }

        string[] serviceTypeNames = serviceTypeNamesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        serviceDescriptors = serviceDescriptors
            .Where(sd =>
                sd.ServiceType.FullName != null &&
                serviceTypeNames.Any(stn => sd.ServiceType.FullName.StartsWith(stn, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return serviceDescriptors;
    }
}
