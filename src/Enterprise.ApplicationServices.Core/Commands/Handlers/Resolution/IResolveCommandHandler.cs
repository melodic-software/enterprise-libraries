using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;

/// <summary>
/// Resolves command handler implementations that can handle specific commands.
/// </summary>
public interface IResolveCommandHandler
{
    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<TCommand> GetCommandHandler<TCommand>(TCommand command) where TCommand : class, ICommand;

    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<TCommand, TResult> GetCommandHandler<TCommand, TResult>(TCommand command)
        where TCommand : class, ICommand<TResult>;
}
