using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility;
using Enterprise.DI.Core.Registration.Abstract;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility.Simple;

public class QueryHandlerRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // The query logic implementation would live in the infrastructure project.
        // Preferably, the following line would also live in a registrar class in the infrastructure project as well.
        services.AddTransient<IQueryLogic<Query, QueryResult>>(_ => new MyQueryLogic());

        // This line should go in an application service layer registrar.
        // Internally it creates an implementation factory that injects in an IQueryLogic implementation.
        services.RegisterSimpleQueryHandler<Query, QueryResult>();
    }
}
