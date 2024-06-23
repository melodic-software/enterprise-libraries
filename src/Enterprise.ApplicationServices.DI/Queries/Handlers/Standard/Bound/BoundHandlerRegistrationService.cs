using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Services.StandardHandlerTypeService;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Bound;

internal static class BoundHandlerRegistrationService
{
    public static void RegisterBound<TQuery, TResult>(IServiceCollection services)
        where TQuery : class, IQuery<TResult>
    {
        Type standardHandlerType = GetStandardHandlerType<TQuery, TResult>();

        ServiceDescriptor? serviceDescriptor = services
            .LastOrDefault(x => x.ServiceType == standardHandlerType);

        if (serviceDescriptor != null)
        {
            // Register the bound.
            services.Add(
                ServiceDescriptor.Describe(
                    typeof(IHandleQuery<TQuery, TResult>),
                    provider => provider.GetRequiredService(standardHandlerType),
                    serviceDescriptor.Lifetime
                )
            );
        }
    }
}
