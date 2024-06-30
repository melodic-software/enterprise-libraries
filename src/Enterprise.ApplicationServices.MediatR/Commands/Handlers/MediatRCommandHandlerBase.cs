using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.ApplicationServices.MediatR.Commands.Handlers;

public abstract class MediatRCommandHandlerBase<TCommand>
    : CommandHandlerBase<TCommand, Result>, IRequestHandler<TCommand, Result>
    where TCommand : class, ICommand<Result>
{
    protected MediatRCommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
