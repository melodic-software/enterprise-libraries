using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Resolution;

/// <summary>
/// Resolves command handler implementations that can handle specific commands.
/// </summary>
public interface IResolveCommandHandler
{
    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand GetHandlerFor(ICommand command);

    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand GetHandlerFor<TResponse>(ICommand<TResponse> command);

    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<TCommand> GetHandlerFor<TCommand>(TCommand command) where TCommand : IBaseCommand;

    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<TCommand, TResponse> GetHandlerFor<TCommand, TResponse>(TCommand command) where TCommand : IBaseCommand;
}
