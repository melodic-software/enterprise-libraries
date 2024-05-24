using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Library.Core.Attributes;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;

/// <summary>
/// Handles commands and returns an explicit result type.
/// According to Bertrand Meyer, commands (in CQS) return void (there is no return value).
/// Typically, the client of the command handler needs to know if the operation succeeded or failed.
/// This is a pragmatic interface that allows a return value.
/// Use of this interface is acceptable; however, <see cref="IHandleCommand"/> is preferred.
/// </summary>
[AlternativeTo(typeof(IHandleCommand))]
public interface IHandleCommand<TResponse> : IHandleCommand
{
    public Task<TResponse> HandleAsync(ICommand<TResponse> command, CancellationToken cancellationToken);
}

/// <summary>
/// Handles commands of a specific type and returns an explicit result type.
/// According to Bertrand Meyer, commands (in CQS) return void (there is no return value).
/// Typically, the client of the command handler needs to know if the operation succeeded or failed.
/// This is a pragmatic interface that allows a return value.
/// Use of this interface is acceptable; however, <see cref="IHandleCommand{T}"/> is preferred.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
[AlternativeTo(typeof(IHandleCommand<>))]
public interface IHandleCommand<in TCommand, TResponse> : IHandleCommand<TResponse> where TCommand : IBaseCommand
{
    /// <summary>
    /// Handle the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
