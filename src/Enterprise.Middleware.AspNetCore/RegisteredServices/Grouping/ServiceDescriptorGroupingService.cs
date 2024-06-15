using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering.Constants;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Grouping;

public static class ServiceDescriptorGroupingService
{
    public static List<ServiceDescriptionDto>? Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors, HttpContext context, ILogger logger)
    {
        if (!query.TryGetValue(QueryStringConstants.GroupByNamespace, out StringValues groupByNamespaceValue) ||
            !bool.TryParse(groupByNamespaceValue, out bool groupByNamespace) || !groupByNamespace)
        {
            return null;
        }

        var groupedResult = serviceDescriptors
            .Where(sd => !string.IsNullOrWhiteSpace(sd.ServiceType.Namespace))
            .GroupBy(sd => sd.ServiceType.Namespace!)
            .OrderBy(g => g.Key)
            .SelectMany(g => g.Select(sd => DtoCreationService.CreateDto(sd, context, logger)))
            .Where(x => x != null)
            .Select(x => x!)
            .OrderBy(x => x.ServiceTypeFullName)
            .ToList();

        return groupedResult;
    }
}
