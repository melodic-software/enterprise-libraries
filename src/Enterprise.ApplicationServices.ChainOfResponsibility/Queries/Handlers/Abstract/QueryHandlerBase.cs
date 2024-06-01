using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers.Abstract;
public abstract class QueryHandlerBase<TQuery, TResponse> : 
    Core.Queries.Handlers.QueryHandlerBase<TQuery, TResponse>, IHandler<TQuery, TResponse> 
    where TQuery : IBaseQuery
{
    protected QueryHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {
    }

    /// <inheritdoc />
    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Core.Queries.Handlers.QueryHandlerBase<TQuery, TResponse> queryHandler = this;
        TResponse response = await queryHandler.HandleAsync(request, cancellationToken);
        return response;
    }
}
