namespace Enterprise.ApplicationServices.Core.Queries.Model;

public interface ICachedQuery : IQuery
{
    /// <summary>
    /// The unique cache identifier associated with the query.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// The total time to live duration.
    /// </summary>
    public TimeSpan? Expiration { get; }
}

public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery;
