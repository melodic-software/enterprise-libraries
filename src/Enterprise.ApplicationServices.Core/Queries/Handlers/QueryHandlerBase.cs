using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public abstract class QueryHandlerBase<TQuery, TResponse> : ApplicationServiceBase, IHandleQuery<TQuery, TResponse>
    where TQuery : IQuery
{

    protected QueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {

    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(IQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResponse result = await HandleAsync(typedQuery, cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
