namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

/// <summary>
/// Resolves command handler implementations that can handle specific commands.
/// </summary>
public interface IResolveCommandHandler
{
    /// <summary>
    /// Get the handler implementation that can handle the given command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="command"></param>
    /// <returns></returns>
    IHandleCommand<T> GetHandlerFor<T>(T command) where T : IBaseCommand;
}