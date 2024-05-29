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
    public IHandleQuery<TResponse>? GetQueryHandler<TResponse>(IQuery query)
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResponse));
        var handler = (IHandleQuery<TResponse>)_serviceProvider.GetService(handlerType);
        return handler;
    }

    /// <inheritdoc />
    public IHandleQuery<TResponse>? GetQueryHandler<TResponse>(IQuery<TResponse> query)
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResponse));
        var handler = (IHandleQuery<TResponse>)_serviceProvider.GetService(handlerType);
        return handler;
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse>? GetQueryHandler<TQuery, TResponse>(TQuery query) where TQuery : IQuery
    {
        return _serviceProvider.GetService<IHandleQuery<TQuery, TResponse>>();
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse>? GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query) where TQuery : IQuery<TResponse>
    {
        return _serviceProvider.GetService<IHandleQuery<TQuery, TResponse>>();
    }
}
