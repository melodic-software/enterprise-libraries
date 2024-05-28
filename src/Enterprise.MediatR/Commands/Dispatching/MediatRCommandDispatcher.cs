using Enterprise.ApplicationServices.Core.Commands.Dispatching;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.MediatR.Commands.Dispatching;

public class MediatRCommandDispatcher : IDispatchCommands
{
    private readonly ISender _sender;

    public MediatRCommandDispatcher(ISender sender)
    {
        _sender = sender;
    }

    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        IRequest<Result> request = command;
        await _sender.Send(request, cancellationToken);
    }

    public async Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        IRequest<Result<TResponse>> request = command;
        Result<TResponse> response = await _sender.Send(request, cancellationToken);
        return response;
    }
}
