using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.CommandHandlers;

internal class CommandHandlerResolutionRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(provider =>
        {
            IResolveCommandHandler commandHandlerResolver = new CommandHandlerResolver(provider);

            return commandHandlerResolver;
        });
    }
}