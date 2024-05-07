using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Controllers.ActionFilters;

internal class ActionFilterServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DevOnlyActionFilter>();
    }
}
