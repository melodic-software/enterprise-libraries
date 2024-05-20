using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.QueryHandlers;

internal sealed class QueryHandlerResolutionServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(provider =>
        {
            IResolveQueryHandler queryHandlerResolver = new QueryHandlerResolver(provider);

            return queryHandlerResolver;
        });
    }
}
