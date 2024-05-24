using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Facade;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Queries.Facade;

internal sealed class QueryFacadeService : IQueryDispatchFacade
{
    private readonly IDispatchQueries _queryDispatcher;
    private readonly IEventCallbackService _eventCallbackService;

    public QueryFacadeService(IDispatchQueries queryDispatcher, IEventCallbackService eventCallbackService)
    {
        _queryDispatcher = queryDispatcher;
        _eventCallbackService = eventCallbackService;
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        _eventCallbackService.RegisterEventCallback(action);
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
    {
        return await _queryDispatcher.DispatchAsync<TQuery, TResponse>(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TResponse>(IQuery query, CancellationToken cancellationToken)
    {
        return await _queryDispatcher.DispatchAsync<TResponse>(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        return await _queryDispatcher.DispatchAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventCallbackService.ClearCallbacks();
    }
}
