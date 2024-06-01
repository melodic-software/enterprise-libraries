using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.DI.Queries.Handlers.Decoration;
using Enterprise.ApplicationServices.Queries.Handlers.Simple;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Facade.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Simple;

public static class SimpleQueryHandlerRegistrationExtensions
{
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
                IEventRaisingFacade eventRaisingFacade = provider.GetRequiredService<IEventRaisingFacade>();

                // Resolve the query logic implementation.
                IQueryLogic<TQuery, TResponse> queryLogic = queryLogicFactory(provider);

                // Use a common handler that delegates to the query logic.
                // We can still add cross-cutting concerns and decorate this handler as needed.
                IHandleQuery<TQuery, TResponse> queryHandler = new SimpleQueryHandler<TQuery, TResponse>(eventRaisingFacade, queryLogic);

                return queryHandler;
            }, serviceLifetime)
            .WithDefaultDecorators();

        // TODO: Decorators? Chain of Responsibility?
    }
}
