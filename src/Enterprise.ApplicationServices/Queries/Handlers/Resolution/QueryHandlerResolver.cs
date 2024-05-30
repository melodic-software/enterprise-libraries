using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Queries.Handlers.Resolution;

public class QueryHandlerResolver : IResolveQueryHandler
{
    private readonly IServiceProvider _serviceProvider;

    public QueryHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IHandleQuery<TResponse> GetQueryHandler<TResponse>(IQuery query)
    {
        return Get<IHandleQuery<TResponse>, NullQueryHandler<TResponse>, TResponse>(query);
    }

    /// <inheritdoc />
    public IHandleQuery<TResponse> GetQueryHandler<TResponse>(IQuery<TResponse> query)
    {
        return Get<IHandleQuery<TResponse>, NullQueryHandler<TResponse>, TResponse>(query);
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query)
        where TQuery : IQuery
    {
        return Get<IHandleQuery<TQuery, TResponse>, NullQueryHandler<TQuery, TResponse>>();
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query)
        where TQuery : IQuery<TResponse>
    {
        return Get<IHandleQuery<TQuery, TResponse>, NullQueryHandler<TQuery, TResponse>>();
    }

    private THandler Get<THandler, TNullHandler, TResponse>(IQuery query)
        where THandler : class where TNullHandler : THandler
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResponse));

        var handler = (THandler)_serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<TNullHandler>();

        return handler;
    }

    private THandler Get<THandler, TNullHandler>() where THandler : class where TNullHandler : THandler
    {
        THandler handler = _serviceProvider.GetService<THandler>();

        if (handler == null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<TNullHandler>();

        return handler;
    }
}
