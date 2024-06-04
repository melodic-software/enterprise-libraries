using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;

public abstract class CommandHandlerBase<TCommand, TResponse>
    : CommandHandlerBase<TCommand>, IHandleCommand<TCommand, TResponse>, IHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    protected CommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    public abstract override Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
