using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Abstract;

public abstract class CommandHandlerBase<TCommand> : 
    Core.Commands.Handlers.CommandHandlerBase<TCommand>, IHandler<TCommand>
    where TCommand : IBaseCommand
{
    protected CommandHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        Core.Commands.Handlers.CommandHandlerBase<TCommand> commandHandler = this;
        await commandHandler.HandleAsync(request, cancellationToken);
    }
}
