using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.MediatR.CommandHandlers;

public abstract class MediatRCommandHandlerBase<TCommand>
    : CommandHandlerBase<TCommand, Result>, IRequestHandler<TCommand, Result>
    where TCommand : IBaseCommand, IRequest<Result>
{
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}

public abstract class MediatRCommandHandlerBase<TCommand, TResponse>
    : CommandHandlerBase<TCommand, TResponse>, IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IBaseCommand, IRequest<Result<TResponse>>
{
    public async Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
