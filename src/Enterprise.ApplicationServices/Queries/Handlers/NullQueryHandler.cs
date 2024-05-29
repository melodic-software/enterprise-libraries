using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.Patterns.ResultPattern.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public class NullQueryHandler<TResponse> : NullQueryHandlerBase, IHandleQuery<TResponse>
{
    public NullQueryHandler(ILogger<NullQueryHandler<TResponse>> logger) : base(logger)
    {

    }

    public Task<Result<TResponse>> HandleAsync(IQuery query, CancellationToken cancellationToken)
    {
        LogWarning(query);
        return Task.FromResult(Result<TResponse>.From(default));
    }
}
