using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Queries.Dispatching;

public class QueryDispatchServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
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
    }
}
