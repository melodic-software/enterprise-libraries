using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Decoration;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public static class RegistrationExtensions
{
    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterQueryHandler<TQuery, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registration = services
            .BeginRegistration<IHandleQuery<TQuery, TResponse>>();

        if (options.UseDecorators)
        {
            registration.RegisterWithDecorators(options);
        }
        else
        {
            registration.AddQueryHandler(options);
        }

        return registration;
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> AddQueryHandler<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registration,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        if (options.QueryHandlerFactory == null)
        {
            throw new InvalidOperationException(
                "A query handler factory delegate must be provided for query handler registrations."
            );
        }

        registration.Add(options.QueryHandlerFactory, options.ServiceLifetime);

        return registration;
    }
}
