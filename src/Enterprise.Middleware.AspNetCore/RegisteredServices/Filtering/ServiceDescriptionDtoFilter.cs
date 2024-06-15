using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ServiceDescriptionDtoFilter
{
    public static List<ServiceDescriptionDto> Execute(IQueryCollection query, List<ServiceDescriptionDto> dtos)
    {
        dtos = NamespaceFilter.Execute(query, dtos);
        dtos = ImplementationTypeFilter.Execute(query, dtos);
        
        return dtos;
    }
}
