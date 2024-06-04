using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommandsWithResult
{
    Task<Result> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<Result>;
}
