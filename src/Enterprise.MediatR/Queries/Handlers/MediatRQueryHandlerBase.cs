using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Events.Facade.Abstract;
using MediatR;

namespace Enterprise.MediatR.Queries.Handlers;

public abstract class MediatRQueryHandlerBase<TQuery, TResponse>
    : QueryHandlerBase<TQuery, TResponse>, IRequestHandler<TQuery, TResponse>
    where TQuery : IRequest<TResponse>, IQuery
{
    protected MediatRQueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }
}
