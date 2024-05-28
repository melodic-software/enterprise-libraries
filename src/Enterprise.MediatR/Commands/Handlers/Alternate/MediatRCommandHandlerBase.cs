using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.MediatR.Commands.Handlers.Alternate;

public abstract class MediatRCommandHandlerBase<TCommand, TResponse>
    : CommandHandlerBase<TCommand, TResponse>, IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
    protected MediatRCommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    public async Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
