using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.DI.Core.Registration.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Commands.Handlers.Resolution;

internal sealed class CommandHandlerResolutionServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            IResolveCommandHandler commandHandlerResolver = new CommandHandlerResolver(provider);

            return commandHandlerResolver;
        });
    }
}
