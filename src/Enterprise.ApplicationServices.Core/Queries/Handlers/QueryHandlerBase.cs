using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.Validation.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public abstract class QueryHandlerBase<TQuery, TResponse> : 
    ApplicationServiceBase, IHandleQuery<TQuery, TResponse>, IHandler<TQuery, TResponse>
    where TQuery : class, IBaseQuery
{
    protected QueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {

    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(IBaseQuery query, CancellationToken cancellationToken)
    {
        ValidateType(query, this);
        var typedQuery = (TQuery)query;
        TResponse response = await HandleAsync(typedQuery, cancellationToken);
        return response;
    }

    /// <inheritdoc />
    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
