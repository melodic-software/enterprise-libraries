using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class NamespaceFilter
{
    public static List<ServiceDescriptionDto> Execute(IQueryCollection query, List<ServiceDescriptionDto> dtos)
    {
        if (!query.TryGetValue(QueryStringConstants.Namespaces, out StringValues namespacesFilterValue))
        {
            return dtos;
        }

        string[] namespaces = namespacesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        dtos = dtos
            .Where(sd =>
                sd.ServiceTypeNamespace != null &&
                namespaces.Any(n => sd.ServiceTypeNamespace.StartsWith(n, StringComparison.OrdinalIgnoreCase)) || 
                sd.ImplementationTypeNamespace != null &&
                namespaces.Any(n => sd.ImplementationTypeNamespace.StartsWith(n, StringComparison.OrdinalIgnoreCase))
                )
            .ToList();

        return dtos;
    }
}
