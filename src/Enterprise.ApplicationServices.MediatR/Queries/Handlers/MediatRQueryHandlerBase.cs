using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.ApplicationServices.MediatR.Queries.Handlers;

public abstract class MediatRQueryHandlerBase<TQuery, TResult>
    : QueryHandlerBase<TQuery, TResult>, IRequestHandler<TQuery, TResult>
    where TQuery : class, IQuery, IRequest<TResult>
{
    protected MediatRQueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
