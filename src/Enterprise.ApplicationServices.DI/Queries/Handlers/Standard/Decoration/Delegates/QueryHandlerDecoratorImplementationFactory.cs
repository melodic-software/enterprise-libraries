using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.Base;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;

public delegate IHandleQuery<TQuery, TResult>
    QueryHandlerDecoratorImplementationFactory<TQuery, TResult>(IServiceProvider provider, IHandleQuery<TQuery, TResult> queryHandler)
    where TQuery : class, IBaseQuery;
