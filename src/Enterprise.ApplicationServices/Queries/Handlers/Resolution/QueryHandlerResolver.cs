﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
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
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery query)
    {
        IServiceProvider serviceProvider = GetServiceProvider();
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResult));
        return (IHandleQuery<TResult>)serviceProvider.GetService(handlerType);
    }

    /// <inheritdoc />
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery<TResult> query)
    {
        IServiceProvider serviceProvider = GetServiceProvider();
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResult));
        return (IHandleQuery<TResult>)serviceProvider.GetRequiredService(handlerType);
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(TQuery query)
        where TQuery : class, IQuery
    {
        IServiceProvider serviceProvider = ScopedProviderService.GetScopedProvider(_serviceProvider);
        return serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResult>>();
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(IQuery<TResult> query)
        where TQuery : class, IQuery<TResult>
    {
        IServiceProvider serviceProvider = ScopedProviderService.GetScopedProvider(_serviceProvider);
        return serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResult>>();
    }
}
