using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class NamespaceFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        if (!query.TryGetValue(QueryStringConstants.Namespaces, out StringValues namespacesFilterValue))
        {
            return serviceDescriptors;
        }

        string[] namespaces = namespacesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        serviceDescriptors = serviceDescriptors
            .Where(sd =>
                sd.ServiceType.Namespace != null &&
                namespaces.Any(n => sd.ServiceType.Namespace.StartsWith(n, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return serviceDescriptors;
    }
}
