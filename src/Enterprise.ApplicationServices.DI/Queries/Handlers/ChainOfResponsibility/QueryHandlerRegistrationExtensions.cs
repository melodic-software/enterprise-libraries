using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Standard;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler using the chain of responsibility design pattern.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="configureOptions"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> factory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        services.Register(factory, configureOptions);
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
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        services.Register(SimpleCommandHandlerFactoryService.GetFactory<TQuery, TResponse>(), configureOptions);
    }

    private static void Register<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, QueryHandlerBase<TQuery, TResponse>> factory,
        Action<RegistrationOptions<TQuery, TResponse>>? configureOptions = null)
        where TQuery : IBaseQuery
    {
        ArgumentNullException.ThrowIfNull(factory);
        var options = new RegistrationOptions<TQuery, TResponse>(factory);
        configureOptions?.Invoke(options);
        RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext = services.RegisterQueryHandler(options);
        options.PostConfigure?.Invoke(services, registrationContext);
    }
}
