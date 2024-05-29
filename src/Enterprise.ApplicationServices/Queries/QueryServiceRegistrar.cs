using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Queries.Dispatching;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.ApplicationServices.Queries.Handlers.Resolution;
using Enterprise.DI.Core.Registration;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Queries;

internal sealed class QueryServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            IResolveQueryHandler queryHandlerResolver = new QueryHandlerResolver(provider);

            return queryHandlerResolver;
        });

        services.AddScoped(provider =>
        {
            IResolveQueryHandler queryHandlerResolver = provider.GetRequiredService<IResolveQueryHandler>();
            IDispatchQueries commandDispatcher = new QueryDispatcher(queryHandlerResolver);
            return commandDispatcher;
        });

        services.AddScoped(provider =>
        {
            IDispatchQueries queryDispatcher = provider.GetRequiredService<IDispatchQueries>();
            IEventCallbackService eventCallbackService = provider.GetRequiredService<IEventCallbackService>();
            IQueryDispatchFacade queryDispatchFacade = new QueryDispatchFacade(queryDispatcher, eventCallbackService);
            return queryDispatchFacade;
        });

        // These are implementations of the null object pattern.
        // They log warnings internally, but this ensures the dispatching services do not have to check for nulls.
        // Registering these makes it easier to resolve them from the DI container.
        services.AddTransient(typeof(NullQueryHandler<>));
        services.AddTransient(typeof(NullQueryHandler<,>));
    }
}
