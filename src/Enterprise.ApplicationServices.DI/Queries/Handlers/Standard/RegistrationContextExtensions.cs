using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

internal static class RegistrationContextExtensions
{
    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> AddQueryHandler<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        if (options.QueryHandlerImplementationFactory == null)
        {
            throw new InvalidOperationException(
                "A query handler implementation factory delegate must be provided for query handler registrations."
            );
        }

        registrationContext.Add(options.QueryHandlerImplementationFactory, options.ServiceLifetime);

        // We can also can register this alternative.
        var alternateImplementationFactory = options.QueryHandlerImplementationFactory 
            as Func<IServiceProvider, IHandleQuery<TResponse>>;

        var serviceDescriptor = new ServiceDescriptor(
            typeof(IHandleQuery<TResponse>),
            alternateImplementationFactory,
            options.ServiceLifetime
        );

        registrationContext.Add(serviceDescriptor);

        return registrationContext;
    }
}
