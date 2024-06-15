using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
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
        where TCommand : class, ICommand
    {
        IServiceProvider serviceProvider = GetServiceProvider();
        return serviceProvider.GetRequiredService<IHandleCommand<TCommand>>();
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand, TResult> GetHandlerFor<TCommand, TResult>(TCommand command)
        where TCommand : class, ICommand<TResult>
    {
        IServiceProvider serviceProvider = GetServiceProvider();
        return serviceProvider.GetRequiredService<IHandleCommand<TCommand, TResult>>();
    }

    private IServiceProvider GetServiceProvider()
    {
        IServiceScopeFactory? scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();

        if (scopeFactory == null)
        {
            return _serviceProvider;
        }

        IServiceScope scope = scopeFactory.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;
        return serviceProvider;
    }
}
