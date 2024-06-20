using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Bound.Delegates;

public delegate QueryHandlerBase<TQuery, TResult>
    QueryHandlerImplementationFactory<TQuery, TResult>(IServiceProvider provider)
    where TQuery : class, IQuery<TResult>;
