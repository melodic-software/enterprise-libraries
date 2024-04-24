using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public abstract class QueryHandlerBase<TQuery, TResponse> : ApplicationServiceBase, IHandleQuery<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    protected QueryHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        TQuery typedQuery = (TQuery)query;
        TResponse result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}