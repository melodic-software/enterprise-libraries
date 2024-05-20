using Enterprise.ApplicationServices.Core.Commands;
using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.MediatR.CommandHandlers;

public abstract class MediatRCommandHandlerBase<TCommand>
    : CommandHandlerBase<TCommand, Result>, IRequestHandler<TCommand, Result>
    where TCommand : IBaseCommand, IRequest<Result>
{
    protected MediatRCommandHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}

public abstract class MediatRCommandHandlerBase<TCommand, TResponse>
    : CommandHandlerBase<TCommand, TResponse>, IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IBaseCommand, IRequest<Result<TResponse>>
{
    protected MediatRCommandHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    public async Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
