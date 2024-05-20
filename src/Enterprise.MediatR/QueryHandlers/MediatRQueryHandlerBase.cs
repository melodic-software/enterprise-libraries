using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using MediatR;

namespace Enterprise.MediatR.QueryHandlers;

public abstract class MediatRQueryHandlerBase<TQuery, TResponse>
    : QueryHandlerBase<TQuery, TResponse>, IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IRequest<Result<TResponse>>, IBaseQuery
{
    protected MediatRQueryHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {
    }

    public async Task<Result<TResponse>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
