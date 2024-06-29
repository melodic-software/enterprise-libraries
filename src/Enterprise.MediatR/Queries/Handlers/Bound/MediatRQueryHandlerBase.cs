using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Generic;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.MediatR.Queries.Handlers.Bound;

public abstract class MediatRQueryHandlerBase<TQuery, TResult>
    : QueryHandlerBase<TQuery, TResult>, IRequestHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>, IRequest<TResult>
{
    protected MediatRQueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
