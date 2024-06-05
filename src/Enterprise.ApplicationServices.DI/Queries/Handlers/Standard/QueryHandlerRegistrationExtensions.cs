using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Delegates;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationFactory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        QueryHandlerImplementationFactory<TQuery, TResponse> implementationFactory,
        ConfigureOptions<TQuery, TResponse>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        services.Register(implementationFactory, configureOptions);
    }

    /// <summary>
    /// Register a simple query handler.
    /// This expects that a separate registration of <see cref="IQueryLogic{TQuery,TResponse}"/> has been made.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterSimpleQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        ConfigureOptions<TQuery, TResponse>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        QueryHandlerImplementationFactory<TQuery, TResponse> implementationFactory =
            QueryHandlerImplementationFactories.CreateSimpleQueryHandler<TQuery, TResponse>;

        services.Register(implementationFactory, configureOptions);
    }

    private static void Register<TQuery, TResponse>(this IServiceCollection services,
        QueryHandlerImplementationFactory<TQuery, TResponse> implementationFactory,
        ConfigureOptions<TQuery, TResponse>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(implementationFactory);
        var options = new RegistrationOptions<TQuery, TResponse>(implementationFactory);
        configureOptions?.Invoke(options);

        RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext =
            services.RegisterQueryHandler(options);

        options.PostConfigure?.Invoke(services, registrationContext);
    }

    private static RegistrationContext<IHandleQuery<TQuery, TResponse>> RegisterQueryHandler<TQuery, TResponse>(
        this IServiceCollection services,
        RegistrationOptions<TQuery, TResponse> options)
        where TQuery : IBaseQuery
    {
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext = services
            .BeginRegistration<IHandleQuery<TQuery, TResponse>>();

        if (options.UseDecorators)
        {
            return registrationContext.RegisterWithDecorators(options);
        }

        return registrationContext.AddQueryHandler(options);
    }
}
