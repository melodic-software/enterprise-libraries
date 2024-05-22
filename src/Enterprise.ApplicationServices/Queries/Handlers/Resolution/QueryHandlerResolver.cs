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
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query) where TQuery : IBaseQuery
    {
        return _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResponse>>();
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query) where TQuery : IQuery<TResponse>
    {
        return _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResponse>>();
    }

    /// <inheritdoc />
    public object GetQueryHandler<TResponse>(IQuery<TResponse> query)
    {
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        object handler = _serviceProvider.GetRequiredService(handlerType);
        return handler;
    }
}
