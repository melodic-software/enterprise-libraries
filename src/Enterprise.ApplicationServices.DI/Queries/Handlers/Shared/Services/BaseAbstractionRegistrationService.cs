using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Services;

internal static class BaseAbstractionRegistrationService
{
    public static void RegisterBaseAbstractions<TQuery, TResult>(IServiceCollection services, ServiceLifetime serviceLifetime) where TQuery : class, IQuery
    {
        // Register the unbound.
        services.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleQuery<TResult>),
                provider => provider.GetRequiredService<IHandleQuery<TQuery, TResult>>(),
                serviceLifetime
            )
        );

        // Register the non-generic.
        services.Add(
            ServiceDescriptor.Describe(
                typeof(IHandleQuery),
                provider => provider.GetRequiredService<IHandleQuery<TQuery, TResult>>(),
                serviceLifetime
            )
        );
    }
}
