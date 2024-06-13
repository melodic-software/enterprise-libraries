using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.Library.Core.Attributes;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;

/// <summary>
/// Handles commands of a specific type and returns an explicit result type.
/// According to Bertrand Meyer, commands (in CQS) return void (there is no return value).
/// Typically, the client of the command handler needs to know if the operation succeeded or failed.
/// This is a pragmatic interface that allows a return value.
/// Use of this interface is acceptable; however, <see cref="IHandleCommand{T}"/> is preferred.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
[AlternativeTo(typeof(IHandleCommand<>))]
public interface IHandleCommand<in TCommand, TResult> : IHandleCommand<TCommand>
    where TCommand : class, ICommand<TResult>
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    new Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
