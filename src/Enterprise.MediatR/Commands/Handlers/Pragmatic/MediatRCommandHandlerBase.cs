using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.MediatR.Commands.Handlers.Pragmatic;

public abstract class MediatRCommandHandlerBase<TCommand, TResult>
    : CommandHandlerBase<TCommand, TResult>, IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    protected MediatRCommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
