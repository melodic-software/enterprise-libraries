using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Decorators.QueryHandlers;
using Enterprise.ApplicationServices.QueryHandlers;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Facade.Abstract;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Extensions;

public static class QueryHandlerRegistrationExtensions
{
    /// <summary>
    /// Register a query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="factory"></param>
    /// <param name="serviceLifetime"></param>
    public static void RegisterQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IHandleQuery<TQuery, TResponse>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TQuery : IBaseQuery
    {
        services.BeginRegistration<IHandleQuery<TQuery, TResponse>>()
            .Add(factory, serviceLifetime)
            .WithDefaultDecorators();
    }

    /// <summary>
    /// Register a simple query handler.
    /// This expects that a separate registration of <see cref="IQueryLogic{TQuery,TResponse}"/> has been made.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    public static void RegisterSimpleQueryHandler<TQuery, TResponse>(this IServiceCollection services) where TQuery : IBaseQuery
    {
        services.RegisterSimpleQueryHandler(provider =>
        {
            IQueryLogic<TQuery, TResponse?> queryLogic = provider.GetRequiredService<IQueryLogic<TQuery, TResponse?>>();

            return queryLogic;
        });
    }


    /// <summary>
    /// Register a simple query handler.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="services"></param>
    /// <param name="queryLogicFactory"></param>
    /// <param name="serviceLifetime"></param>
    public static void RegisterSimpleQueryHandler<TQuery, TResponse>(this IServiceCollection services,
        Func<IServiceProvider, IQueryLogic<TQuery, TResponse>> queryLogicFactory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TQuery : IBaseQuery
    {
        services.BeginRegistration<IHandleQuery<TQuery, TResponse>>()
            .Add(provider =>
            {
                IEventServiceFacade eventServiceFacade = provider.GetRequiredService<IEventServiceFacade>();

                // Resolve the query logic implementation.
                IQueryLogic<TQuery, TResponse> queryLogic = queryLogicFactory(provider);

                // Use a common handler that delegates to the query logic.
                // We can still add cross-cutting concerns and decorate this handler as needed.
                IHandleQuery<TQuery, TResponse> queryHandler = new SimpleQueryHandler<TQuery, TResponse>(eventServiceFacade, queryLogic);

                return queryHandler;
            }, serviceLifetime)
            .WithDefaultDecorators();
    }

    private static void WithDefaultDecorators<TQuery, TResponse>(this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext)
        where TQuery : IBaseQuery
    {
        registrationContext
            .WithDecorators((provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TQuery>> validators = provider.GetServices<IValidator<TQuery>>();
                IHandleQuery<TQuery, TResponse> decorator = new FluentValidationQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, validators);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleQuery<TQuery, TResponse> decorator = new NullQueryValidationQueryHandler<TQuery, TResponse>(queryHandler, decoratorService);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> logger = provider.GetRequiredService<ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>>>();
                IHandleQuery<TQuery, TResponse> decorator = new ErrorHandlingQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, logger);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingQueryHandler<TQuery, TResponse>> logger = provider.GetRequiredService<ILogger<LoggingQueryHandler<TQuery, TResponse>>>();
                IHandleQuery<TQuery, TResponse> decorator = new LoggingQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, logger);
                return decorator;
            });
    }
}