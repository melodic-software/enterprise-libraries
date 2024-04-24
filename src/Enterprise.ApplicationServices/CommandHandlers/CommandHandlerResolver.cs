using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.CommandHandlers;

public class CommandHandlerResolver : IResolveCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IHandleCommand<T> GetHandlerFor<T>(T command) where T : IBaseCommand
    {
        IHandleCommand<T> commandHandler = _serviceProvider.GetRequiredService<CommandHandlerBase<T>>();

        return commandHandler;
    }
}