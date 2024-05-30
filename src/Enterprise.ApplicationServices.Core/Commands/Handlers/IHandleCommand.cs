using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

/// <summary>
/// Handles commands.
/// </summary>
public interface IHandleCommand : IApplicationService
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Handles commands of a specific type.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface IHandleCommand<in TCommand> : IHandleCommand where TCommand : IBaseCommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
