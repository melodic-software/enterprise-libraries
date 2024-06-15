using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;

public static class ServiceDescriptorFilter
{
    public static List<ServiceDescriptor> Execute(IQueryCollection query, List<ServiceDescriptor> serviceDescriptors)
    {
        serviceDescriptors = ServiceLifetimeFilter.Apply(query, serviceDescriptors);
        serviceDescriptors = ServiceKeyFilter.Execute(query, serviceDescriptors);
        serviceDescriptors = ServiceTypeFilter.Execute(query, serviceDescriptors);
        serviceDescriptors = MicrosoftNamespaceFilter.Execute(query, serviceDescriptors);
        serviceDescriptors = NamespaceFilter.Execute(query, serviceDescriptors);

        return serviceDescriptors;
    }
}
