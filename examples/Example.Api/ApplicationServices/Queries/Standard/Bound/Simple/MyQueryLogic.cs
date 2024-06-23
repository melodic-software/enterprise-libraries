using Enterprise.ApplicationServices.Core.Queries.Handlers.Bound;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound.Simple;

// This would live in an infrastructure project and use direct data access components.
// There is no need for a repository or any indirection there.

public class MyQueryLogic : IQueryLogic<BoundQuery, QueryResult>
{
    public Task<QueryResult> ExecuteAsync(BoundQuery query, CancellationToken cancellationToken = default)
    {
        var result = new QueryResult("BOUND QUERY LOGIC");
        return Task.FromResult(result);
    }
}
