using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;
using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Events.Facade.Abstract;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility;

public class QueryHandlerRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterQueryHandler(provider =>
        {
            IEventRaisingFacade eventService = provider.GetRequiredService<IEventRaisingFacade>();
            return new QueryHandler(eventService);
        });
    }
}
