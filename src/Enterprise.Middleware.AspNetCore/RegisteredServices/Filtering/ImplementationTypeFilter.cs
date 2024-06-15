using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ImplementationTypeFilter
{
    public static List<ServiceDescriptionDto> Execute(IQueryCollection query, List<ServiceDescriptionDto> dtos)
    {
        if (!query.TryGetValue(QueryStringConstants.ImplementationTypeNames, out StringValues implementationTypeNamesFilterValue))
        {
            return dtos;
        }

        string[] implementationTypeNames = implementationTypeNamesFilterValue.ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();

        dtos = dtos
            .Where(dto =>
                dto.ImplementationTypeFullName != null &&
                implementationTypeNames.Any(itn => dto.ImplementationTypeFullName.StartsWith(itn, StringComparison.OrdinalIgnoreCase)) ||
                dto.ImplementationTypeName != null &&
                implementationTypeNames.Any(itn => dto.ImplementationTypeName.Contains(itn, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();

        return dtos;
    }
}
