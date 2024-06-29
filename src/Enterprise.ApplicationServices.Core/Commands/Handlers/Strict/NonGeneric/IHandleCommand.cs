using Enterprise.ApplicationServices.Core.Commands.Model.Base;
using Enterprise.ApplicationServices.Core.Standard;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Strict.NonGeneric;

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
    public Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken = default);
}
