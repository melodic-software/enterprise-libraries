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
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResponse));

        var handler = (IHandleQuery<TResponse>)_serviceProvider.GetService(handlerType);

        if (handler != null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<NullQueryHandler<TResponse>>();

        return handler;
    }

    /// <inheritdoc />
    public IHandleQuery<TResponse> GetQueryHandler<TResponse>(IQuery<TResponse> query)
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResponse));

        var handler = (IHandleQuery<TResponse>)_serviceProvider.GetService(handlerType);

        if (handler != null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<NullQueryHandler<TResponse>>();

        return handler;
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query)
        where TQuery : IQuery
    {
        IHandleQuery<TQuery, TResponse>? handler = _serviceProvider.GetService<IHandleQuery<TQuery, TResponse>>();

        if (handler != null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<NullQueryHandler<TQuery, TResponse>>();

        return handler;
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQuery<TResponse> query)
        where TQuery : IQuery<TResponse>
    {
        IHandleQuery<TQuery, TResponse>? handler = _serviceProvider.GetService<IHandleQuery<TQuery, TResponse>>();

        if (handler != null)
        {
            return handler;
        }

        handler = _serviceProvider.GetRequiredService<NullQueryHandler<TQuery, TResponse>>();

        return handler;
    }
}
