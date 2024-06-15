using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Microsoft.Extensions.DependencyInjection;
using static Enterprise.DI.Core.ServiceProviders.ScopedProviderService;

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
        IServiceProvider serviceProvider = GetScopedProvider(_serviceProvider);
        return serviceProvider.GetRequiredService<IHandleCommand<TCommand>>();
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand, TResult> GetHandlerFor<TCommand, TResult>(TCommand command)
        where TCommand : class, ICommand<TResult>
    {
        IServiceProvider serviceProvider = GetScopedProvider(_serviceProvider);
        return serviceProvider.GetRequiredService<IHandleCommand<TCommand, TResult>>();
    }
}
