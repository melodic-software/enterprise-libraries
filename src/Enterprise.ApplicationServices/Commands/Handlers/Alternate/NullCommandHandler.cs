using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Patterns.ResultPattern.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Commands.Handlers.Alternate;

public class NullCommandHandler<TCommand, TResponse> : NullCommandHandlerBase, IHandleCommand<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public NullCommandHandler(ILogger<NullCommandHandler<TCommand, TResponse>> logger) : base(logger)
    {

    }

    Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        return HandleAsync(command, cancellationToken);
    }

    public Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        LogWarning(command);
        return Task.FromResult(Result<TResponse>.From(default));
    }

    public Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        LogWarning(command);
        return Task.CompletedTask;
    }
}
