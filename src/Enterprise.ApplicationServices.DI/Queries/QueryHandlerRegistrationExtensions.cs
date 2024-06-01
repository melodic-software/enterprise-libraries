using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.ChainOfResponsibility;
using Enterprise.ApplicationServices.DI.Queries.Decoration;
using Enterprise.ApplicationServices.DI.Queries.Options;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IHandleQuery<TQuery, TResponse>> factory,
        Action<QueryHandlerRegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new QueryHandlerRegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        services.RegisterQueryHandler( options);
    }

    /// <summary>
    /// Register a query handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IHandler<TQuery, TResponse>> factory,
        Action<QueryHandlerRegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new QueryHandlerRegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        services.RegisterQueryHandler(options);
    }

    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        QueryHandlerRegistrationOptions<TQuery, TResponse> options)
        where TQuery : IQuery
    {
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registration = services
            .BeginRegistration<IHandleQuery<TQuery, TResponse>>();

        // We only allow one type of registration. Using multiple methods wouldn't make sense.

        if (options.UseChainOfResponsibility)
        {
            registration.AddChainOfResponsibility(options, services);
        }
        else if (options.UseDecorators)
        {
            registration.RegisterWithDecorators(options);
        }
        else
        {
            registration.AddQueryHandler(options);
        }

        options.PostConfigure?.Invoke(services, registration);
    }

    internal static RegistrationContext<IHandleQuery<TQuery, TResponse>> AddQueryHandler<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registration,
        QueryHandlerRegistrationOptions<TQuery, TResponse> options)
        where TQuery : IQuery
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
