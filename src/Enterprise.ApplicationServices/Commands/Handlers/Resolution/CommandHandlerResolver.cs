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
        // Check if the current provider is already a scoped service provider.
        if (_serviceProvider.GetService<IServiceScopeFactory>() == null)
        {
            return _serviceProvider;
        }

        if (_serviceProvider is IServiceScope)
        {
            return _serviceProvider;
        }

        // Create a new scope if we are in the root scope.
        IServiceScopeFactory scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        IServiceScope scope = scopeFactory.CreateScope();
        return scope.ServiceProvider;
    }
}
