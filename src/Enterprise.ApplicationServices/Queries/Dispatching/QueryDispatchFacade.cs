using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.ApplicationServices.Core.Queries.Model.Generic;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.Events.Callbacks.Facade.Abstractions;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Queries.Dispatching;

public class QueryDispatchFacade : IQueryDispatchFacade
{
    private readonly IDispatchQueries _queryDispatcher;
    private readonly IEventCallbackService _eventCallbackService;

    public QueryDispatchFacade(IDispatchQueries queryDispatcher, IEventCallbackService eventCallbackService)
    {
        _queryDispatcher = queryDispatcher;
        _eventCallbackService = eventCallbackService;
    }

    /// <inheritdoc />
    public void RegisterEventCallback<TEvent>(Action<TEvent> action)
        where TEvent : IEvent
    {
        _eventCallbackService.RegisterEventCallback(action);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class, IQuery
    {
        return await _queryDispatcher.DispatchAsync<TQuery, TResult>(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TResult>(IQuery query, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.DispatchAsync<TResult>(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.DispatchAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public void ClearCallbacks()
    {
        _eventCallbackService.ClearCallbacks();
    }
}
