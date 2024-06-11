using Enterprise.DesignPatterns.Decorator.Services;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DesignPatterns.Decorator.Dependencies;

internal sealed class DecoratorServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            IGetDecoratedInstance decoratorService = new DecoratedInstanceService();

            return decoratorService;
        });
    }
}
