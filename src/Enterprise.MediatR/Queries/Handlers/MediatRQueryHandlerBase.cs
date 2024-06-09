using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.MediatR.Queries.Handlers;

public abstract class MediatRQueryHandlerBase<TQuery, TResult>
    : QueryHandlerBase<TQuery, TResult>, IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>, IQuery
{
    protected MediatRQueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
