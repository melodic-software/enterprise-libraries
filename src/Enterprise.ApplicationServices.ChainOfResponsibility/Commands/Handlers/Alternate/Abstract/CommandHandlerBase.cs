using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate.Abstract;
public abstract class CommandHandlerBase<TCommand, TResponse> :
    Core.Commands.Handlers.Alternate.CommandHandlerBase<TCommand, TResponse>, IHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    protected CommandHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Core.Commands.Handlers.Alternate.CommandHandlerBase<TCommand, TResponse> commandHandler = this;
        TResponse response = await commandHandler.HandleAsync(request, cancellationToken);
        return response;
    }
}
