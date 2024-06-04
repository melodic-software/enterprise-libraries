using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommandsWithResponse : IDispatchCommandsWithResult
{
    Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>;
}
