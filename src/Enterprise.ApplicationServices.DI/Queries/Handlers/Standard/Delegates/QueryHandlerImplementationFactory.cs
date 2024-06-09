using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;

public delegate QueryHandlerBase<TQuery, TResult>
    QueryHandlerImplementationFactory<TQuery, TResult>(IServiceProvider provider)
    where TQuery : class, IQuery;

public delegate IHandleQuery<TResult>
    QueryHandlerImplementationFactory<TResult>(IServiceProvider provider);
