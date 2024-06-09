using Enterprise.ApplicationServices.Core.Commands.Handlers.NonGeneric;
using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

/// <summary>
/// Handles commands of a specific type.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface IHandleCommand<in TCommand> : IHandleCommand where TCommand : class, ICommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
