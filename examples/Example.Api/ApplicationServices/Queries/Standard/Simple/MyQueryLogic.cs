

using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Simple;

// This would live in an infrastructure project and use direct data access components.
// There is no need for a repository or any indirection there.

public class MyQueryLogic : IQueryLogic<Query, QueryResult>
{
    public Task<QueryResult> ExecuteAsync(Query query, CancellationToken cancellationToken = default)
    {
        var result = new QueryResult("SIMPLE QUERY LOGIC");
        return Task.FromResult(result);
    }
}
