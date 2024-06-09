using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

internal static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleQuery<TQuery, TResult>> AddQueryHandler<TQuery, TResult>(
        this RegistrationContext<IHandleQuery<TQuery, TResult>> registrationContext,
        RegistrationOptions<TQuery, TResult> options)
        where TQuery : IBaseQuery
    {
        if (options.QueryHandlerImplementationFactory == null)
        {
            throw new InvalidOperationException(
                "A query handler implementation factory delegate must be provided for query handler registrations."
            );
        }

        // Register the primary.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleQuery<TQuery, TResult>),
                options.QueryHandlerImplementationFactory.Invoke,
                options.ServiceLifetime
            )
        );

        // Register the alternate.
        registrationContext.Add(
            new ServiceDescriptor(
                typeof(IHandleQuery<TResult>),
                options.QueryHandlerImplementationFactory.Invoke,
                options.ServiceLifetime
            )
        );

        return registrationContext;
    }
}
