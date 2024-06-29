using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict.NonGeneric;
using Enterprise.ApplicationServices.Core.Commands.Model.Base;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;

/// <summary>
/// Handles commands of a specific type.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface IHandleCommand<in TCommand> : IHandleCommand where TCommand : class, IBaseCommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
