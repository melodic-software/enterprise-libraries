using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Queries.Handlers.Resolution;

internal sealed class QueryHandlerResolutionServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            IResolveQueryHandler queryHandlerResolver = new QueryHandlerResolver(provider);

            return queryHandlerResolver;
        });
    }
}
