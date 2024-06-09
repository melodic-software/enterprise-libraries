using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;

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
    IHandleCommand<TCommand> GetHandlerFor<TCommand>(TCommand command) where TCommand : class, ICommand;

    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<TCommand, TResponse> GetHandlerFor<TCommand, TResponse>(TCommand command)
        where TCommand : class, ICommand<TResponse>;
}
