using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Controllers.ActionFilters;

public class ActionFilterRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DevOnlyActionFilter>();
    }
}