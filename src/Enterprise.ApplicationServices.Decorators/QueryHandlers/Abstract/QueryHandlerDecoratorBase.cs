using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using static Enterprise.ApplicationServices.Core.Queries.Handlers.QueryHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;

public abstract class QueryHandlerDecoratorBase<TQuery, TResponse> : DecoratorBase<IHandleQuery<TQuery, TResponse>>,
    IHandleQuery<TQuery, TResponse> where TQuery : IBaseQuery
{
    protected QueryHandlerDecoratorBase(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService)
        : base(queryHandler, decoratorService)
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

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        Decorated.RegisterEventCallback(eventCallback);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        Decorated.ClearCallbacks();
    }
}