namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

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
