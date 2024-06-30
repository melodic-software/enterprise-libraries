using System.Reflection;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict.NonGeneric;
using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Commands.Handlers.Resolution;

public class CommandHandlerResolver : IResolveCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IHandleCommand GetHandlerFor(IBaseCommand command)
    {
        Type commandType = command.GetType();
        Type[] commandInterfaces = commandType.GetInterfaces();

        Type pragmaticInterface = Array.Find(commandInterfaces,
            t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommand<>)
        );

        if (pragmaticInterface != null)
        {
            Type resultType = pragmaticInterface.GetGenericArguments()[0];
            Type handlerType = typeof(IHandleCommand<,>).MakeGenericType(commandType, resultType);
            return (IHandleCommand)_serviceProvider.GetRequiredService(handlerType);
        }

        if (typeof(ICommand).IsAssignableFrom(commandType))
        {
            Type nonResultHandlerType = typeof(IHandleCommand<>).MakeGenericType(commandType);
            return (IHandleCommand)_serviceProvider.GetRequiredService(nonResultHandlerType);
        }

        throw new NotSupportedException($"Command handler cannot be resolved for command type: {commandType.Name}");
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand> GetHandlerFor<TCommand>(TCommand command)
        where TCommand : class, ICommand
    {
        return _serviceProvider.GetRequiredService<IHandleCommand<TCommand>>();
    }

    /// <inheritdoc />
    public IHandleCommand<TCommand, TResult> GetHandlerFor<TCommand, TResult>(TCommand command)
        where TCommand : class, ICommand<TResult>
    {
        return _serviceProvider.GetRequiredService<IHandleCommand<TCommand, TResult>>();
    }
}
