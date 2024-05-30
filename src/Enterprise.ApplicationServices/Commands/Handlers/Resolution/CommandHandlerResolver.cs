using Enterprise.ApplicationServices.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Commands.Handlers.Resolution;

public class CommandHandlerResolver : IResolveCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand> GetHandlerFor<TCommand>(TCommand command)
        where TCommand : IBaseCommand
    {
        return Get<IHandleCommand<TCommand>, NullCommandHandler<TCommand>>();
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand, TResponse> GetHandlerFor<TCommand, TResponse>(TCommand command)
        where TCommand : ICommand<TResponse>
    {
        return Get<IHandleCommand<TCommand, TResponse>, NullCommandHandler<TCommand, TResponse>>();
    }

    private THandler Get<THandler, TNullHandler>() where THandler : class where TNullHandler : THandler
    {
        THandler handler = _serviceProvider.GetService<THandler>();

        if (handler != null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<TNullHandler>();

        return handler;
    }
}
